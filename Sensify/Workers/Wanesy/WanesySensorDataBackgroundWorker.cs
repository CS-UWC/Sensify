using EnumToStringGenerator;
using Microsoft.Extensions.Options;
using Sensify.Grains;
using Sensify.Grains.Senors.Common;
using Sensify.Persistence;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;
using Sensify.Extensions;
using MongoDB.Driver.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using MongoDB.Driver;

namespace Sensify.Workers.Wanesy;

internal class WanesySensorDataBackgroundWorker : BackgroundService
{
    private const string _basePath = "https://alliot.wanesy.com";
    private const string _loginPath = "/gms/application/login";
    private const string _dataUpPath = "/gms/application/dataUp";
    private const string _cacheResultsId = $"{_basePath}{_dataUpPath}";
    private readonly ILogger<WanesySensorDataBackgroundWorker> _logger;
    private readonly IGrainFactory _grainFactory;
    private readonly IMongoPersistenceProvider _mongoPersistenceProvider;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly WanesyCredentials _credentials;

    private readonly Random _random = new();

    private readonly JsonSerializerOptions _jsonSerializerOptions = new(JsonSerializerDefaults.Web);
    private readonly MediaTypeHeaderValue _jsonHeaderValue = new(MediaTypeNames.Application.Json);
    private readonly ISensorManagerGrain _sensorManagerGrain;

    private readonly IMongoCollection<SearchResultCache> _searchCacheCollection;


    public WanesySensorDataBackgroundWorker(
        ILogger<WanesySensorDataBackgroundWorker> logger,
        IOptions<WanesyCredentials> options,
        IGrainFactory grainFactory,
        IMongoPersistenceProvider mongoPersistenceProvider,
        IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _grainFactory = grainFactory;
        _mongoPersistenceProvider = mongoPersistenceProvider;
        _httpClientFactory = httpClientFactory;
        _jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        _credentials = options.Value;
        _sensorManagerGrain = _grainFactory.GetGrain<ISensorManagerGrain>(Guid.Empty);
        _searchCacheCollection = _mongoPersistenceProvider.GetCollection<SearchResultCache>("searchCache");
    }

    private async ValueTask<SensorInfo[]> GetSensorInfos()
    {
        try
        {
            var sensorInfos = await _sensorManagerGrain.GetSensors();

            return sensorInfos
                .Where(x => !string.IsNullOrWhiteSpace(x?.Id?.Id))
                .ToArray();
        }catch(Exception e)
        {
            _logger.LogError(e, "Error getting sensor infos");
        }

        return [];
    }

    private async Task<SearchResultCache?> GetLastCacheResults()
    {
        try
        {
            var id = $"{_basePath}{_dataUpPath}";
            return await _searchCacheCollection.Find(x => x.Id == id)
                .Limit(1)
                .FirstOrDefaultAsync();

        }catch(Exception e)
        {
            _logger.LogError(e, "Error getting search cache results");
        }

        return null;
    }

    private async Task SaveCacheResults(SearchResultCache results)
    {
        try
        {
            await _searchCacheCollection.FindOneAndReplaceAsync(x => x.Id == _cacheResultsId, results, new() { IsUpsert = true });

        }catch(Exception e)
        {
            _logger.LogError(e, "Error getting search cache results");
        }

    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        await Task.Delay(TimeSpan.FromHours(1 / 3600.0), stoppingToken);
        LoginResponse? loginResponse = null;
        LoginRequest loginRequest = new(_credentials.Username, _credentials.Password);

        DateTime tokenExpiresAt = DateTime.UnixEpoch;

        while (!stoppingToken.IsCancellationRequested)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_basePath);

            loginResponse ??= await Login(client, loginRequest, stoppingToken);
            if (loginResponse is null || loginResponse.GetExpiredDate() < DateTimeOffset.UtcNow.Subtract(TimeSpan.FromMinutes(10)))
            {
                // token expired or failed to login

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); // wait 1 minute and try again
                continue;
            }

            // we are logged in

            var sensors = await GetSensorInfos();

            if(sensors is { Length: 0 })
            {
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                continue;
            }

            var deviceEuisMap = sensors.ToDictionary(x => x.Id!.Value.Id);

            var cachedResults = await GetLastCacheResults();

            var query = cachedResults?.LastQuery;

            if(query is null)
            {

                SearchQuery sq = new( Operand: "endDevice.devEui", Operation: SearchOperation.In, Values: [.. deviceEuisMap.Keys]);

                query = new DataUpQuery("-recvTime", 1, 40, sq);

            }

            var dataUpRequest = new HttpRequestMessage(HttpMethod.Get, $"{_dataUpPath}?={query.ToQueryString()}");

            var scheme = string.IsNullOrWhiteSpace(loginResponse.Token) ? "Bearer" : loginResponse.TokenType;
            dataUpRequest.Headers.Authorization = new AuthenticationHeaderValue(scheme, loginResponse.Token);

            var response = await SendAndReadJson<DataUpResponse>(client, dataUpRequest, stoppingToken);

            if (response is null or { Count: 0})
            {
                // failed to get results, maybe the server is down or there was an error
                await Task.Delay(TimeSpan.FromMinutes(_random.Next(1, 5)), stoppingToken);
                continue;
            }

            if(response.List is { Count: 0 })
            {
                // no data

                await Task.Delay(TimeSpan.FromMinutes(_random.Next(25, 35) / 5.0), stoppingToken);
                continue;
            }

            try
            {

                var skip = 0;

                if(cachedResults is { LastPageResultCount: > 0 } && cachedResults.LastPageResultCount < cachedResults.LastQuery.PageSize)
                {
                    // partial page from the last query
                    skip = cachedResults.LastPageResultCount;
                }

                var sensorDataUpsMap = response.List
                    .Skip(skip)
                    .Where(x => !string.IsNullOrWhiteSpace(x?.EndDevice?.DevEui))
                    .Where(x => deviceEuisMap.ContainsKey(x.EndDevice.DevEui))
                    .Select(x => (sensor: deviceEuisMap[x.EndDevice.DevEui], dataUp: x))
                    .GroupBy(x => x.sensor)
                    .ToDictionary(x => x.Key);

                var count = 0;

                foreach (var (sensorInfo, groupItems) in sensorDataUpsMap)
                {
                    var _sensor = _grainFactory.GetGrain<ISensor>(sensorInfo.Id.ToString());
                    foreach(var (_, dataUp) in groupItems)
                    {
                        count++;
                        var rawMeasurement = new RawSensorMeasurement(dataUp.Payload, DateTimeOffset.FromUnixTimeMilliseconds(dataUp.RecvTime).UtcDateTime);
                        _ = _sensor.UpdateMeasurementAsync(rawMeasurement);
                    }
                }

                var nextPage = response.Count == response.PageSize ? query.Page + 1 : query.Page; 
                query = query with { Page = nextPage };
                cachedResults ??= new(_cacheResultsId, LastPageResultCount: response.Count, query);

                cachedResults = cachedResults with { LastPageResultCount = response.Count, LastQuery =  query };

                await SaveCacheResults(cachedResults);

                // wait a bit longer for new real-timer data
                // when e have reached the end.

                TimeSpan wait = query.Page > response.nbPages ? TimeSpan.FromMinutes(5) : TimeSpan.FromSeconds(_random.Next(5, 50) / 5f);

                await Task.Delay(wait, stoppingToken);

            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to get data");
            }
        }
    }

    private ValueTask<LoginResponse?> Login(HttpClient httpClient, LoginRequest loginRequest, CancellationToken cancellationToken = default)
    {

        var body = JsonSerializer.Serialize(loginRequest, _jsonSerializerOptions);
        var request = new HttpRequestMessage(HttpMethod.Post, _loginPath)
        {
            Content = new StringContent(body, _jsonHeaderValue)
        };

        return SendAndReadJson<LoginResponse>(httpClient, request, cancellationToken);
    }

    private async ValueTask<T?> SendAndReadJson<T>(HttpClient httpClient, HttpRequestMessage requestMessage, CancellationToken cancellationToken = default)
    {

        try
        {
            var response = await httpClient.SendAsync(requestMessage, cancellationToken);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<T>(_jsonSerializerOptions, cancellationToken);
        }
        catch(Exception e)
        {
            _logger.LogError(e, "Error making request to {path} and reading json.", requestMessage.RequestUri);
        }

        return default;
    }
}

internal record SearchResultCache(string Id, int LastPageResultCount, DataUpQuery LastQuery);

[GenerateStrings]
public enum SearchOperator
{
    [JsonPropertyName("AND")]
    And,
    [JsonPropertyName("OR")]
    Or
}

[GenerateStrings]
public enum SearchOperation
{
    [JsonPropertyName("EQ")]
    Eq,
    [JsonPropertyName("IN")]
    In
}

internal record SearchConditions(SearchOperation Operation, List<string> Values, string Operand)
{
    public string ToJsonString()
    {
        return $$"""
            {"operation":"{{Operation.AsString()}}","values":[{{string.Join(',', Values.Select(JsonString))}}],"operand":{{JsonString(Operand)}}}
            """;


        static string JsonString(string st) => $"\"{st}\"";
    }
}
internal record SearchQuery(
    SearchOperator? Operator = null,
    string? Operand = null,
    SearchOperation? Operation = null,
    List<string>? Values = null,
    List<SearchConditions>? Conditions = null)
{
    public string ToQueryString()
    {
        string json = string.Empty;
        if(Operator is null && !string.IsNullOrWhiteSpace(Operand) && Operation is not null && Values is { Count: > 0})
        {
            json = $$"""
            {"operand":"{{Operand}}","operation": "{{Operation!.Value.AsString()}}","values":[{{string.Join(',', Values.Select(JsonString))}}]}
            """;
        }
        else if(Operator is not null && Conditions is { Count: > 0 })
        {
            json = $$"""
            {"operator":"{{Operator.Value.AsString()}}","conditions":[{{string.Join(',', Conditions.Select(x => x.ToJsonString()))}}]}
            """;
        }

        return HttpUtility.UrlEncode(json);

        static string JsonString(string st) => $"\"{st}\"";
    }
}
internal record DataUpQuery(
    string Sort,
    uint Page,
    uint PageSize,
    SearchQuery? Search = default
)
{

    public string ToQueryString()
    {
        if(Search is null)
        {
            return $"sort={Sort}&page={Page}&pageSize={PageSize}";
        }

        return $"sort={Sort}&page={Page}&pageSize={PageSize}&search={Search.ToQueryString()}";
    }
}

file record ClusterInfo(int Id);

file record EndDevice(string DevAddr, ClusterInfo Cluster, string DevEui);

file record Link(string Rel, string Href);

file record DataUpInfo(
    string Id,
    EndDevice EndDevice,
    bool Pushed,
    int Fport,
    int FCntDown,
    int FCntUp,
    bool Confirmed,
    string Payload,
    bool Encrypted,
    float UlFrequency,
    string Modulation,
    string DataRate,
    long RecvTime,
    long GwRecvTime,
    int GwCnt,
    bool Adr,
    string CodingRate,
    bool Delayed,
    bool ClassB,
    string EncodingType);

file record DataUpResponse(
    int Count,
    int Page,
    int PageSize,
    List<Link> Links,
    int TotalCount,
    List<DataUpInfo> List,
    int nbPages);

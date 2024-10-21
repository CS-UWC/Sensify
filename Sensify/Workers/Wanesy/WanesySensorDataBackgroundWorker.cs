using Amazon.Auth.AccessControlPolicy;
using EnumToStringGenerator;
using Microsoft.Extensions.Options;
using Sensify.Grains;
using Sensify.Grains.Senors.Common;
using Sensify.Persistence;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Web;
using Sensify.Extensions;
using MongoDB.Driver.Linq;
using MongoDB.Bson;

namespace Sensify.Workers.Wanesy;

internal class WanesySensorDataBackgroundWorker : BackgroundService
{
    private const string _loginPath = "/gms/application/login";
    private const string _dataUpPath = "/gms/application/dataUp";
    private readonly ILogger<WanesySensorDataBackgroundWorker> _logger;
    private readonly IGrainFactory _grainFactory;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly WanesyCredentials _credentials;

    private readonly Random _random = new();

    private readonly JsonSerializerOptions _jsonSerializerOptions = new(JsonSerializerDefaults.Web);
    private readonly MediaTypeHeaderValue _jsonHeaderValue = new(MediaTypeNames.Application.Json);
    private readonly ISensorManagerGrain _sensorManagerGrain;


    public WanesySensorDataBackgroundWorker(
        ILogger<WanesySensorDataBackgroundWorker> logger,
        IOptions<WanesyCredentials> options,
        IGrainFactory grainFactory,
        IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _grainFactory = grainFactory;
        _httpClientFactory = httpClientFactory;
        _jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        _credentials = options.Value;
        _sensorManagerGrain = _grainFactory.GetGrain<ISensorManagerGrain>(Guid.Empty);
    }

    private async ValueTask<SensorInfo[]> GetSensorInfos()
    {
        try
        {
            var sensorInfos = await _sensorManagerGrain.GetSensors();

            return sensorInfos
                .Where(x => x.Id is not null)
                .Where(x => !string.IsNullOrWhiteSpace(x.Id?.Id))
                .ToArray();
        }catch(Exception e)
        {
            _logger.LogError(e, "Error getting sensor infos");
        }

        return Array.Empty<SensorInfo>();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        await Task.Delay(TimeSpan.FromHours(10), stoppingToken);
        LoginResponse? loginResponse = null;
        LoginRequest loginRequest = new(_credentials.Username, _credentials.Password);

        DateTime tokenExpiresAt = DateTime.UnixEpoch;

        while (!stoppingToken.IsCancellationRequested)
        {
            var client = _httpClientFactory.CreateClient();

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

            SearchQuery sq = new(SearchOperator.And, [new(SearchOperation.Eq, [.. deviceEuisMap.Keys], "endDevice.devEui")]);

            var query = new DataUpQuery("-recvTime", 1, 20, sq);
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

            try
            {


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

[GenerateStrings]
public enum SearchOperator
{
    [JsonPropertyName("AND")]
    And
}

[GenerateStrings]
public enum SearchOperation
{
    [JsonPropertyName("eq")]
    Eq
}

file record SearchConditions(SearchOperation Operation, List<string> Values, string Operand)
{
    public string ToJsonString()
    {
        return $$"""
            {"operation":{{Operation.AsString()}},"values":[{{string.Join(',', Values)}}],"operand:"{{Operand}}"}
            """;
    }
}
file record SearchQuery(SearchOperator Operator, List<SearchConditions> Conditions)
{
    public string ToQueryString()
    {
        var json = $$"""
            {"operator":"{{Operator.AsString()}}","conditions":["{{string.Join(',', Conditions.Select(x => x.ToJsonString()))}}"]}
            """;

        return HttpUtility.UrlEncode(json);
    }
}
file record DataUpQuery(
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
    DateTime RecvTime,
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

using MongoDB.Bson;
using MongoDB.Driver;
using Orleans;
using Orleans.Concurrency;
using Sensify.Decoders.Common;
using Sensify.Extensions;
using Sensify.Grains.Senors.Common;
using Sensify.Persistence;
using System.Collections.Concurrent;

namespace Sensify.Grains;

[Reentrant]
public class SensorManagerGrain : Grain, ISensorManagerGrain
{
    private readonly ConcurrentDictionary<SensorId, ISensor> _sensors = new();

    private readonly IMongoCollection<BsonDocument> _sensorsDb;

    private readonly IFindFluent<BsonDocument, BsonValue> _findFluent;

    private readonly SemaphoreSlim _semaphore = new(1);
    private readonly ILogger<SensorManagerGrain> _logger;

    private IGrainTimer? _grainTimer;

    public SensorManagerGrain(
        IMongoPersistenceProvider persistenceProvider,
        ILogger<SensorManagerGrain> logger,
        IGrainContext grainContext,
        IGrainRuntime? grainRuntime = null) 
        : base(grainContext, grainRuntime)
    {
        _sensorsDb = persistenceProvider.GetCollection<BsonDocument>("GrainssensorInfo");
        _findFluent = _sensorsDb.Find(Builders<BsonDocument>.Filter.Empty)
            .Project(Builders<BsonDocument>.Projection.Expression(x => x["_id"]));
        _logger = logger;
    }

    public override async Task OnActivateAsync(CancellationToken cancellationToken)
    {
        _grainTimer = this.RegisterGrainTimer(OnTimer, new GrainTimerCreationOptions(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1)));
        await base.OnActivateAsync(cancellationToken);
    }

    private async ValueTask GetSensorIdsFromDb(bool isActivate = false)
    {
        if (!isActivate && _sensors is { Count: > 0 }) return;

        await _semaphore.WaitAsync();

        if (!isActivate &&  _sensors is { Count: > 0 }) return;
        try
        {
            await foreach (var v in _findFluent.GetAsyncEnumerable())
            {
                if (!TryParseSensorId(v.AsString, out var sensorId)) continue;

                _sensors.GetOrAdd(sensorId, GrainFactory.GetGrain<ISensor>(sensorId.ToString()).AsReference<ISensor>());
            };

        }catch(Exception e)
        {
            _logger.LogError(e, "Something went wrong loading the sensors");
        }
        finally { _semaphore.Release(); }
    }

    private static bool TryParseSensorId(ReadOnlySpan<char> rawIdValue, out SensorId sensorId)
    {
        var index = rawIdValue.LastIndexOf('/');

        sensorId = default;

        if(index is -1) return false;

        rawIdValue = rawIdValue[(index+1)..];

        if(!SensorId.IsValid(rawIdValue)) return false;

        sensorId = SensorId.From(rawIdValue);

        return true;


    }

    public ValueTask AddSensor(SensorId sensorId, ISensor sensor)
    {
        if (_sensors.ContainsKey(sensorId)) return ValueTask.CompletedTask;

        _sensors[sensorId] = sensor;

        return ValueTask.CompletedTask;
    }

    public async ValueTask<IEnumerable<SensorInfo>> GetSensors()
    {
        await GetSensorIdsFromDb();
        var sensorListTask = _sensors.Values
            .Select(x => x.GetSensorInfoAsync().AsTask())
            .ToList();

        return await Task.WhenAll(sensorListTask);
    }

    public async Task OnTimer()
    {
        await GetSensorIdsFromDb(true);
        _grainTimer?.Dispose();
        _grainTimer = null;
    }
}

[Alias("Sensify.Grains.ISensorManagerGrain")]
public interface ISensorManagerGrain : IGrainWithGuidKey
{
    ValueTask<IEnumerable<SensorInfo>> GetSensors();

    ValueTask AddSensor(SensorId sensorId, ISensor sensor);
}

using MongoDB.Driver;
using Sensify.Decoders.Netvox;
using Sensify.Grains.Sensors.Common;
using Sensify.Persistence;

namespace Sensify.Grains.NetvoxSensorGrain;

internal sealed partial class NetvoxSensorMethods : ISensorMethods
{
    private readonly IPersistentState<SensorInfo> _state;
    private readonly IMongoPersistenceProvider _persistenceProvider;
    private readonly IGrainContext _grainContext;
    private readonly NetvoxDecoder _decoder = new();
    private readonly IMongoCollection<SensorMeasurement<NetvoxMeasurement>> _measurements;

    public NetvoxSensorMethods(
        IPersistentState<SensorInfo> state,
        IMongoPersistenceProvider persistenceProvider,
        IGrainContext grainContext)
    {
        _state = state;
        _persistenceProvider = persistenceProvider;
        _grainContext = grainContext;
        _measurements = _persistenceProvider.GetCollection<SensorMeasurement<NetvoxMeasurement>>("sensorData");
    }

    public ValueTask<string> GetIdAsync()
    {
        return ValueTask.FromResult(_state.State.Id.ToString()!);
    }

    public ValueTask<SensorInfo> GetSensorInfoAsync()
    {
        return ValueTask.FromResult(_state.State);
    }

    public async ValueTask UpdateMeasurementAsync(RawSensorMeasurement raw)
    {
        //Console.WriteLine($"hexPayload: {hexPayload}");

        var data = _decoder.Decode(raw.HexPayload);

        if (data is null) return;

        SensorMeasurement<NetvoxMeasurement> sensorData = new()
        {
            SensorId = _state.State.Id.ToString()!,
            Timestamp = raw.Timestamp ?? DateTime.UtcNow,
            Measurement = data
        };

        await _measurements.InsertOneAsync(sensorData);
    }

    public async ValueTask UpdateSensorInfoAsync(UpdateSensorInfo update)
    {
        var state = _state.State;

        _state.State = state with
        {
            SensorName = update.SensorName,
            PayloadDecoder = update.PayloadDecoder ?? state.PayloadDecoder,
            SensorType = update.SensorType ?? state.SensorType,
        };

        await _state.WriteStateAsync();
    }
}

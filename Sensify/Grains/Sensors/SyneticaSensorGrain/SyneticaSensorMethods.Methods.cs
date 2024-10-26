using MongoDB.Driver;
using Sensify.Decoders.Synetica;
using Sensify.Grains.Sensors.Common;
using Sensify.Persistence;
using System.Runtime.CompilerServices;
using System.Threading.Channels;

namespace Sensify.Grains.SyneticaSensorGrain;

internal sealed partial class SyneticaSensorMethods : ISensorMethods
{
    private readonly IPersistentState<SensorInfo> _state;
    private readonly IMongoPersistenceProvider _persistenceProvider;
    private readonly IGrainContext _grainContext;
    private readonly SyneticaDecoder _decoder = new();
    private readonly IMongoCollection<SensorMeasurement<SyneticaMeasurement>> _measurements;
    private readonly Channel<SensorMeasurement<SyneticaMeasurement>> _LiveQueue= Channel.CreateUnbounded<SensorMeasurement<SyneticaMeasurement>>();
    private ulong _liveStreamsCount = 0; 

    public SyneticaSensorMethods(
        IPersistentState<SensorInfo> state,
        IMongoPersistenceProvider persistenceProvider,
        IGrainContext grainContext)
    {
        _state = state;
        _persistenceProvider = persistenceProvider;
        _grainContext = grainContext;
        _measurements = _persistenceProvider.GetCollection<SensorMeasurement<SyneticaMeasurement>>("sensorData");
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

        var data = _decoder.Decode(raw.HexPayload);

        if (data is null) return;

        SensorMeasurement<SyneticaMeasurement> sensorData = new()
        {
            SensorId = _state.State.Id.ToString()!,
            Timestamp = raw.Timestamp ?? DateTime.UtcNow,
            Measurement = data
        };

        if(_liveStreamsCount > 0)
        {
            _LiveQueue.Writer.TryWrite(sensorData);
        }

        await _measurements.InsertOneAsync(sensorData);
    }

    private void IncrementLiveStreamsCount()
    {
        Interlocked.Increment(ref _liveStreamsCount);
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

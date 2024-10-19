using Sensify.Extensions;
using Sensify.Grains.ElsysSensorGrain;
using Sensify.Persistence;

namespace Sensify.Grains;

public sealed partial class GenericSensor : Grain, ISensor
{
    private readonly IPersistentState<SensorInfo> _state;
    private readonly IMongoPersistenceProvider _persistenceProvider;

    private ISensorMethods? _sensorMethods;

    public GenericSensor(
        [PersistentState(
        stateName: "sensorInfo",
        storageName: "sensorInfo")]
        IPersistentState<SensorInfo> state,
        IMongoPersistenceProvider persistenceProvider,
        IGrainContext grainContext,
        IGrainRuntime? grainRuntime = null)
        : base(grainContext, grainRuntime)
    {
        _state = state;
        _persistenceProvider = persistenceProvider;

    }

    public override async Task OnActivateAsync(CancellationToken cancellationToken)
    {
        if (_state.State is { Id: null } || string.IsNullOrWhiteSpace(_state.State?.Id))
        {
            _state.State = new SensorInfo()
            {
                Id = GrainContext.GrainId.Key.ToString()!
            };

            await _state.WriteStateAsync();
        }

        if (_state.State is not { SensorType: SupportedSensorType.None })
        {
            _sensorMethods = LoadSensorMethods(_state.State.SensorType);
        }
        await base.OnActivateAsync(cancellationToken);
    }

    private ISensorMethods? LoadSensorMethods(SupportedSensorType sensorType)
    {
        return sensorType switch
        {
            SupportedSensorType.Elsys => new ElsysSensorMethods(_state, _persistenceProvider, GrainContext),
            _ => null,

        };
    }

    public ValueTask<string> GetIdAsync()
    {
        return _sensorMethods.WhenNotNull(static x => x!.GetIdAsync())
            .Else(static () => ValueTask.FromResult(string.Empty));
    }

    public async ValueTask UpdateMeasurementAsync(string hexPayload)
    {
        await _sensorMethods.WhenNotNull(x => x!.UpdateMeasurementAsync(hexPayload))
            .Else(static () => ValueTask.CompletedTask);
    }

    public ValueTask<SensorInfo> GetSensorInfoAsync()
    {
        return _sensorMethods.WhenNotNull(static x => x.GetSensorInfoAsync())
            .Else(static () => ValueTask.FromResult(SensorInfo.Empty));
    }

    public async ValueTask UpdateSensorInfoAsync(UpdateSensorInfo update)
    {
        await _sensorMethods.WhenNotNull(x => x!.UpdateSensorInfoAsync(update))
            .Else(static () => ValueTask.CompletedTask);
    }

    public IAsyncEnumerable<object> GetMeasurementsAsync(SensorMeasurementDateRange dateRange = default, MeasurementWindow window = default)
    {
        return _sensorMethods.WhenNotNull(x => x!.GetMeasurementsAsync(dateRange, window))
            .Else(static () => CompletedAsyncEnumerable<object>());
    }

    private static async IAsyncEnumerable<T> CompletedAsyncEnumerable<T>()
    {
        await ValueTask.CompletedTask;
        yield break;
    }
}

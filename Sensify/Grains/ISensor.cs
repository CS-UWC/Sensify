using Orleans.Concurrency;

namespace Sensify.Grains;

internal interface ISensor<TMeasurement> : ISensorMethods
{
    ValueTask<TMeasurement> GetTypedMeasurementsAsync(SensorMeasurementDateRange dateRange = default, MeasurementWindow window = default);

}

[Alias("Sensify.Grains.ISensor")]
public interface ISensor : IGrainWithStringKey, ISensorMethods
{

}

public interface ISensorMethods
{
    [AlwaysInterleave]
    ValueTask<string> GetIdAsync();
    [AlwaysInterleave]
    ValueTask<SensorInfo> GetSensorInfoAsync();
    ValueTask UpdateSensorInfoAsync(UpdateSensorInfo update);
    ValueTask UpdateMeasurementAsync(string hexPayload);
    [AlwaysInterleave]
    IAsyncEnumerable<object> GetMeasurementsAsync(SensorMeasurementDateRange dateRange = default, MeasurementWindow window = default);
}
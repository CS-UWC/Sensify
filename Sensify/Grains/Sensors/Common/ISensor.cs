using Orleans.Concurrency;

namespace Sensify.Grains.Sensors.Common;

[Alias("Sensify.Grains.Sensors.Common.ISensor")]
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
    ValueTask UpdateMeasurementAsync(RawSensorMeasurement raw);
    [AlwaysInterleave]
    IAsyncEnumerable<object> GetMeasurementsAsync(SensorMeasurementDateRange dateRange = default, MeasurementWindow window = default);
}
using Orleans.Concurrency;

namespace Sensify.Grains.Sensors.Common;

public interface ISensorMethods
{
    [AlwaysInterleave]
    ValueTask<string> GetIdAsync();
    [AlwaysInterleave]
    ValueTask<SensorInfo> GetSensorInfoAsync();
    ValueTask UpdateSensorInfoAsync(UpdateSensorInfo update);
    ValueTask UpdateMeasurementAsync(RawSensorMeasurement raw);
    [AlwaysInterleave]
    IAsyncEnumerable<object> GetMeasurementsAsync(SensorMeasurementDateRange dateRange = default, MeasurementWindow window = default, CancellationToken cancellationToken = default);
    [AlwaysInterleave]
    IAsyncEnumerable<object> GetMetricsAsync(CancellationToken cancellationToken = default);
}
using MongoDB.Driver;
using Sensify.Decoders.Synetica;
using Sensify.Extensions;
using Sensify.Persistence;
using System.Runtime.CompilerServices;

namespace Sensify.Grains.SyneticaSensorGrain;

internal sealed partial class SyneticaSensorMethods
{
    public async IAsyncEnumerable<object> GetMetricsAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var asyncEnumerable = GetMeasurementsAsync(cancellationToken: cancellationToken)
            .Transform(x => (SensorMeasurement<SyneticaMeasurement>)x, cancellationToken)
            .Window(20, cancellationToken);


        await foreach (var items in asyncEnumerable)
        {
            if (items is { Length: 0 }) continue;

            var measurements = items.Select(x => (SyneticaMeasurementMetric?)x.Measurement).ToArray();
            SyneticaMeasurementMetric? min = measurements.Aggregate((lhs, rhs) => lhs.Min(rhs));
            SyneticaMeasurementMetric? max = measurements.Aggregate((lhs, rhs) => lhs.Max(rhs));
            SyneticaMeasurementMetric? sum = measurements.Aggregate((lhs, rhs) => lhs + rhs);

            SyneticaMeasurementMetric? average = sum / items.Length;

            SyneticaMeasurementMetric? std = null;
            string? sensorId = null;
            DateTime? timestamp = null;

            foreach (var item in items)
            {
                var diff = item.Measurement - average;
                std += diff * diff;
                sensorId ??= item.SensorId;
                timestamp ??= item.Timestamp;
            }

            std /= items.Length > 1 ? items.Length - 1 : items.Length;
            std = std.Sqrt();
            var range = max - min;
            var metric = SyneticaMetric.FromMeasurementMetrics(min, max, average, range, std);

            yield return new SensorMeasurement<SyneticaMetric> { SensorId = sensorId!, Timestamp = timestamp!.Value, Measurement = metric };

        }

    }
        

}
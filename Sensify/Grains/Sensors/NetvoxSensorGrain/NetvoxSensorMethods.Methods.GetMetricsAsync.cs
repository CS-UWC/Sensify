using MongoDB.Driver;
using Sensify.Decoders.Netvox;
using Sensify.Decoders.Synetica;
using Sensify.Extensions;
using Sensify.Persistence;
using System.Runtime.CompilerServices;

namespace Sensify.Grains.NetvoxSensorGrain;

internal sealed partial class NetvoxSensorMethods
{
    public async IAsyncEnumerable<object> GetMetricsAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {

        var asyncEnumerable = GetMeasurementsAsync(cancellationToken: cancellationToken)
            .Transform(x => (SensorMeasurement<NetvoxMeasurement>)x, cancellationToken)
            .Window(20, cancellationToken);


        await foreach (var items in asyncEnumerable)
        {
            if (items is { Length: 0 }) continue;

            var measurements = items.Select(x => (NetvoxMeasurementMetric?)x.Measurement).ToArray();
            NetvoxMeasurementMetric? min = measurements.Aggregate((lhs, rhs) => lhs.Min(rhs));
            NetvoxMeasurementMetric? max = measurements.Aggregate((lhs, rhs) => lhs.Max(rhs));
            NetvoxMeasurementMetric? sum = measurements.Aggregate((lhs, rhs) => lhs + rhs);

            NetvoxMeasurementMetric? average = sum / items.Length;

            NetvoxMeasurementMetric? std = null;
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
            var metric = NetvoxMetric.FromMeasurementMetrics(min, max, average, range, std);

            yield return new SensorMeasurement<NetvoxMetric> { SensorId = sensorId!, Timestamp = timestamp!.Value, Measurement = metric };

        }

    }
        

}
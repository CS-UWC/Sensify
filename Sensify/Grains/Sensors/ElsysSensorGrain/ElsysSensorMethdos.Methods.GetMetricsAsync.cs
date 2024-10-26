using MongoDB.Driver;
using Sensify.Decoders.Elsys;
using Sensify.Extensions;
using Sensify.Persistence;
using System.Runtime.CompilerServices;

namespace Sensify.Grains.ElsysSensorGrain;

internal sealed partial class ElsysSensorMethods 
{
    public async IAsyncEnumerable<object> GetMetricsAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {

        var sensorFilter = Builders<SensorMeasurement<ElsysMeasurement>>.Filter.Eq(x => x.SensorId, _state.State.Id.ToString());

        var sort = Builders<SensorMeasurement<ElsysMeasurement>>.Sort.Ascending(x => x.Timestamp);

        var asyncEnumerable = GetMeasurementsAsync(cancellationToken: cancellationToken)
            .Transform(x => (SensorMeasurement<ElsysMeasurement>)x, cancellationToken)
            .Window(20, cancellationToken);


        await foreach (var items in asyncEnumerable)
        {
            if (items is { Length: 0 }) continue;

            var measurements = items.Select(x => (ElsysMeasurementMetric?)x.Measurement).ToArray();
            ElsysMeasurementMetric? min = measurements.Aggregate((lhs, rhs) => lhs.Min(rhs));
            ElsysMeasurementMetric? max = measurements.Aggregate((lhs, rhs) => lhs.Max(rhs));
            ElsysMeasurementMetric? sum = measurements.Aggregate((lhs, rhs) => lhs + rhs);

            ElsysMeasurementMetric? average = sum / items.Length;

            ElsysMeasurementMetric? std = null;
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
            var metric = ElsysMetric.FromMeasurementMetrics(min, max, average, range, std);

            yield return new SensorMeasurement<ElsysMetric> { SensorId = sensorId!, Timestamp = timestamp!.Value, Measurement = metric };

        }

    }
        

}
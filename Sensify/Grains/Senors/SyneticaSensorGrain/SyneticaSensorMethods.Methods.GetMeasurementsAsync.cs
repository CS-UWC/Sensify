using MongoDB.Driver;
using Sensify.Decoders.Netvox;
using Sensify.Decoders.Synetica;
using Sensify.Extensions;
using Sensify.Grains.Senors.Common;
using Sensify.Persistence;
using System.Runtime.CompilerServices;

namespace Sensify.Grains.SyneticaSensorGrain;

internal sealed partial class SyneticaSensorMethods
{
    public IAsyncEnumerable<object> GetMeasurementsAsync(SensorMeasurementDateRange dateRange = default, MeasurementWindow window = default)
    {
        List<FilterDefinition<SensorMeasurement<SyneticaMeasurement>>> filters = [];

        var sensorFilter = Builders<SensorMeasurement<SyneticaMeasurement>>.Filter.Eq(x => x.SensorId, _state.State.Id.ToString());
        filters.Add(sensorFilter);

        if(dateRange != SensorMeasurementDateRange.All)
        {
            var startingDateFilter = Builders<SensorMeasurement<SyneticaMeasurement>>.Filter.Gte(x => x.Timestamp, dateRange.Start);
            filters.Add(startingDateFilter);
            var endDateFilter = Builders<SensorMeasurement<SyneticaMeasurement>>.Filter.Lte(x => x.Timestamp, dateRange.End);
            filters.Add(endDateFilter);
        }

        var combined = Builders<SensorMeasurement<SyneticaMeasurement>>.Filter.And(filters);
        var sort = Builders<SensorMeasurement<SyneticaMeasurement>>.Sort.Ascending(x => x.Timestamp);

        var findFluent = _measurements.Find(combined)
            .Sort(sort);

        if (window != MeasurementWindow.None)
        {
            return new WindowedAsyncEnumerable(findFluent, window);
        }

        return findFluent.GetAsyncEnumerable()
            .Transform(x => (object)x);
        
    }

}

file readonly struct WindowedAsyncEnumerable : IAsyncEnumerable<object>
{
    private readonly IFindFluent<SensorMeasurement<SyneticaMeasurement>, SensorMeasurement<SyneticaMeasurement>> _findFluent;
    private readonly MeasurementWindow _window;

    public WindowedAsyncEnumerable(IFindFluent<SensorMeasurement<SyneticaMeasurement>, SensorMeasurement<SyneticaMeasurement>> findFluent, MeasurementWindow window)
    {
        _findFluent = findFluent;
        _window = window;
    }

    public IAsyncEnumerator<object> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {

        var windowedValues = _window.Type switch
        {
            MeasurementWindowType.Integral => _findFluent.GetAsyncEnumerator(cancellationToken)
            .Window(Math.Max(1, Math.Min(60, _window.WindowSize))),
            _ => _findFluent.GetAsyncEnumerator(cancellationToken)
            .Window(CreateDurationWindow(TimeSpan.FromMinutes(1).Max(TimeSpan.FromHours(6).Min(_window.WindowDuration))), cancellationToken)
        };

        return windowedValues.GetAsyncEnumerator(cancellationToken)
            .Transform(static batch => (object)batch.Aggregate(Aggregate), cancellationToken);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static Func<SensorMeasurement<SyneticaMeasurement>?, SensorMeasurement<SyneticaMeasurement>, IReadOnlyList<SensorMeasurement<SyneticaMeasurement>>, bool>
            CreateDurationWindow(TimeSpan windowDuration)
        {
            return (SensorMeasurement<SyneticaMeasurement>? previous, SensorMeasurement<SyneticaMeasurement> current, IReadOnlyList<SensorMeasurement<SyneticaMeasurement>> accumulated) =>
            {
                if (previous is null || (current.Timestamp - previous.Timestamp) < windowDuration) return false;

                return true;

            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static SensorMeasurement<SyneticaMeasurement> Aggregate(SensorMeasurement<SyneticaMeasurement> left, SensorMeasurement<SyneticaMeasurement> right)
        {

            if(left is null)
            {
                return right;
            }

            if(right is null)
            {
                return left;
            }

            left.Timestamp = right.Timestamp;
            left.SensorId ??= right.SensorId;
            left.Measurement += right.Measurement;

            return left;
        }


    }

}
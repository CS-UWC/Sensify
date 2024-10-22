using MongoDB.Driver;
using Sensify.Decoders.Netvox;
using Sensify.Extensions;
using Sensify.Grains.Senors.Common;
using Sensify.Persistence;
using System.Runtime.CompilerServices;

namespace Sensify.Grains.NetvoxSensorGrain;

internal sealed partial class NetvoxSensorMethods
{
    public IAsyncEnumerable<object> GetMeasurementsAsync(SensorMeasurementDateRange dateRange = default, MeasurementWindow window = default)
    {
        List<FilterDefinition<SensorMeasurement<NetvoxMeasurement>>> filters = [];

        var sensorFilter = Builders<SensorMeasurement<NetvoxMeasurement>>.Filter.Eq(x => x.SensorId, _state.State.Id.ToString());
        filters.Add(sensorFilter);

        if(dateRange != SensorMeasurementDateRange.All)
        {
            var startingDateFilter = Builders<SensorMeasurement<NetvoxMeasurement>>.Filter.Gte(x => x.Timestamp, dateRange.Start);
            filters.Add(startingDateFilter);
            var endDateFilter = Builders<SensorMeasurement<NetvoxMeasurement>>.Filter.Lte(x => x.Timestamp, dateRange.End);
            filters.Add(endDateFilter);
        }

        var combined = Builders<SensorMeasurement<NetvoxMeasurement>>.Filter.And(filters);
        var sort = Builders<SensorMeasurement<NetvoxMeasurement>>.Sort.Ascending(x => x.Timestamp);

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
    private readonly IFindFluent<SensorMeasurement<NetvoxMeasurement>, SensorMeasurement<NetvoxMeasurement>> _findFluent;
    private readonly MeasurementWindow _window;

    public WindowedAsyncEnumerable(IFindFluent<SensorMeasurement<NetvoxMeasurement>, SensorMeasurement<NetvoxMeasurement>> findFluent, MeasurementWindow window)
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
        static Func<SensorMeasurement<NetvoxMeasurement>?, SensorMeasurement<NetvoxMeasurement>, IReadOnlyList<SensorMeasurement<NetvoxMeasurement>>, bool>
            CreateDurationWindow(TimeSpan windowDuration)
        {
            return (SensorMeasurement<NetvoxMeasurement>? previous, SensorMeasurement<NetvoxMeasurement> current, IReadOnlyList<SensorMeasurement<NetvoxMeasurement>> accumulated) =>
            {
                if (previous is null || (current.Timestamp - previous.Timestamp) < windowDuration) return false;

                return true;

            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static SensorMeasurement<NetvoxMeasurement> Aggregate(SensorMeasurement<NetvoxMeasurement> left, SensorMeasurement<NetvoxMeasurement> right)
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
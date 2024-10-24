using MongoDB.Driver;
using Sensify.Decoders.Elsys;
using Sensify.Extensions;
using Sensify.Grains.Sensors.Common;
using Sensify.Persistence;
using System.Runtime.CompilerServices;

namespace Sensify.Grains.ElsysSensorGrain;

internal sealed partial class ElsysSensorMethods 
{
    public IAsyncEnumerable<object> GetMeasurementsAsync(SensorMeasurementDateRange dateRange = default, MeasurementWindow window = default)
    {
        List<FilterDefinition<SensorMeasurement<ElsysMeasurement>>> filters = [];

        var sensorFilter = Builders<SensorMeasurement<ElsysMeasurement>>.Filter.Eq(x => x.SensorId, _state.State.Id.ToString());
        filters.Add(sensorFilter);

        if(dateRange != SensorMeasurementDateRange.All)
        {
            var startingDateFilter = Builders<SensorMeasurement<ElsysMeasurement>>.Filter.Gte(x => x.Timestamp, dateRange.Start);
            filters.Add(startingDateFilter);
            var endDateFilter = Builders<SensorMeasurement<ElsysMeasurement>>.Filter.Lte(x => x.Timestamp, dateRange.End);
            filters.Add(endDateFilter);
        }

        var combined = Builders<SensorMeasurement<ElsysMeasurement>>.Filter.And(filters);
        var sort = Builders<SensorMeasurement<ElsysMeasurement>>.Sort.Ascending(x => x.Timestamp);

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
    private readonly IFindFluent<SensorMeasurement<ElsysMeasurement>, SensorMeasurement<ElsysMeasurement>> _findFluent;
    private readonly MeasurementWindow _window;

    public WindowedAsyncEnumerable(IFindFluent<SensorMeasurement<ElsysMeasurement>, SensorMeasurement<ElsysMeasurement>> findFluent, MeasurementWindow window)
    {
        _findFluent = findFluent;
        _window = window;
    }

    public IAsyncEnumerator<object> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        //return _window.Type switch
        //{
        //    MeasurementWindowType.Integral => new CountWindowValueProducer(_findFluent, _window).GetAsyncEnumerator(cancellationToken),
        //    _ => new DurationWindowValueProducer(_findFluent, _window).GetAsyncEnumerator(cancellationToken),
        //};

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
        static Func<SensorMeasurement<ElsysMeasurement>?, SensorMeasurement<ElsysMeasurement>, IReadOnlyList<SensorMeasurement<ElsysMeasurement>>, bool>
            CreateDurationWindow(TimeSpan windowDuration)
        {
            return (SensorMeasurement<ElsysMeasurement>? previous, SensorMeasurement<ElsysMeasurement> current, IReadOnlyList<SensorMeasurement<ElsysMeasurement>> accumulated) =>
            {
                if (previous is null || (current.Timestamp - previous.Timestamp) < windowDuration) return false;

                return true;

            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static SensorMeasurement<ElsysMeasurement> Aggregate(SensorMeasurement<ElsysMeasurement> left, SensorMeasurement<ElsysMeasurement> right)
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

file readonly struct CountWindowValueProducer : IAsyncEnumerable<SensorMeasurement<ElsysMeasurement>>
{
    private readonly IFindFluent<SensorMeasurement<ElsysMeasurement>, SensorMeasurement<ElsysMeasurement>> _findFluent;
    private readonly MeasurementWindow _window;

    public CountWindowValueProducer(
        IFindFluent<SensorMeasurement<ElsysMeasurement>, SensorMeasurement<ElsysMeasurement>> findFluent,
        MeasurementWindow window)
    {
        _findFluent = findFluent;
        _window = window;
    }

    public async IAsyncEnumerator<SensorMeasurement<ElsysMeasurement>> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        var cursor = await _findFluent.ToCursorAsync(cancellationToken);

        var windowSize = Math.Max(1, Math.Min(60, _window.WindowSize));

        if(windowSize == 1)
        {

            while (await cursor.MoveNextAsync(cancellationToken))
            {
                foreach(var doc in cursor.Current)
                {
                    yield return doc;
                }

            }

            // have read everything stop
            yield break;
        }

        SensorMeasurement<ElsysMeasurement>? previousMeasurement = null;
        var consumedCount = 0;
        while (await cursor.MoveNextAsync(cancellationToken))
        {

            var enumerator = cursor.Current.GetEnumerator();

            while (enumerator.MoveNext())
            {
                var current = enumerator.Current;
                consumedCount++;

                if(previousMeasurement is null)
                {
                    previousMeasurement = current;
                    continue;
                }


                if(windowSize == consumedCount)
                {
                    consumedCount = 0;
                    var v = previousMeasurement;
                    previousMeasurement = null;
                    yield return v;
                    continue;
                }

                previousMeasurement.Timestamp = current.Timestamp;
                previousMeasurement.SensorId ??= current.SensorId;
                previousMeasurement.Measurement += current.Measurement;
            }

        }

        if(previousMeasurement is not null) yield return previousMeasurement;
    }
}

file readonly struct DurationWindowValueProducer : IAsyncEnumerable<SensorMeasurement<ElsysMeasurement>>
{
    private readonly IFindFluent<SensorMeasurement<ElsysMeasurement>, SensorMeasurement<ElsysMeasurement>> _findFluent;
    private readonly MeasurementWindow _window;

    public DurationWindowValueProducer(
        IFindFluent<SensorMeasurement<ElsysMeasurement>, SensorMeasurement<ElsysMeasurement>> findFluent,
        MeasurementWindow window)
    {
        _findFluent = findFluent;
        _window = window;
    }
    public async IAsyncEnumerator<SensorMeasurement<ElsysMeasurement>> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        var cursor = await _findFluent.ToCursorAsync(cancellationToken);

        var windowSize = TimeSpan.FromTicks(Math.Max(TimeSpan.FromMinutes(1).Ticks, Math.Min(TimeSpan.FromHours(6).Ticks, _window.WindowDuration.Ticks)));

        SensorMeasurement<ElsysMeasurement>? previousMeasurement = null;
        
        DateTime windowStartValueTimeStamp = DateTime.MaxValue;

        while (await cursor.MoveNextAsync(cancellationToken))
        {

            var enumerator = cursor.Current.GetEnumerator();

            while (enumerator.MoveNext())
            {
                var current = enumerator.Current;

                if(previousMeasurement is null)
                {
                    previousMeasurement = current;
                    windowStartValueTimeStamp = current.Timestamp;
                    continue;
                }

                if ((current.Timestamp - windowStartValueTimeStamp) >= windowSize)
                {
                    var v = previousMeasurement;
                    previousMeasurement = null;
                    yield return v;
                    continue;
                }

                previousMeasurement.Timestamp = current.Timestamp;
                previousMeasurement.SensorId ??= current.SensorId;
                previousMeasurement.Measurement += current.Measurement;
            }

        }

        if (previousMeasurement is not null) yield return previousMeasurement;
    }
}

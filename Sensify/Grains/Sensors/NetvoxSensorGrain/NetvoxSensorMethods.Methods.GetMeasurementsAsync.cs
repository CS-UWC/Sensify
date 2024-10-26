using MongoDB.Driver;
using Sensify.Decoders.Netvox;
using Sensify.Decoders.Synetica;
using Sensify.Extensions;
using Sensify.Grains.Sensors.Common;
using Sensify.Persistence;
using System.Runtime.CompilerServices;

namespace Sensify.Grains.NetvoxSensorGrain;

internal sealed partial class NetvoxSensorMethods
{
    public IAsyncEnumerable<object> GetMeasurementsAsync(SensorMeasurementDateRange dateRange = default, MeasurementWindow window = default, CancellationToken cancellationToken = default)
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

        var asyncEnumerable = _measurements.Find(combined)
            .Sort(sort)
            .GetAsyncEnumerable(cancellationToken);

        IAsyncEnumerable<object> dbSource = asyncEnumerable.Transform(x => (object)x, cancellationToken: cancellationToken);

        if (window != MeasurementWindow.None)
        {
            dbSource = new WindowedAsyncEnumerable(asyncEnumerable, window);
        }

        IncrementLiveStreamsCount();
        return MergeWithLiveSource(dbSource, cancellationToken);

    }


    private async IAsyncEnumerable<object> MergeWithLiveSource(IAsyncEnumerable<object> dbSource, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        SensorMeasurement<NetvoxMeasurement>? lastDBItem = null;

        await foreach (var item in dbSource)
        {
            lastDBItem = item as SensorMeasurement<NetvoxMeasurement>;
            yield return item;
        }

        if (lastDBItem is null)
        {
            await foreach (var item in _liveQueue.Reader.ReadAllAsync(cancellationToken))
            {
                yield return item;
            }

            yield break;
        }

        await foreach (var item in _liveQueue.Reader.ReadAllAsync(cancellationToken))
        {
            if (lastDBItem.Timestamp > item.Timestamp) continue;

            yield return item;
        }
    }

}

file readonly struct WindowedAsyncEnumerable : IAsyncEnumerable<object>
{

    private readonly IAsyncEnumerable<SensorMeasurement<NetvoxMeasurement>> _source;
    private readonly MeasurementWindow _window;
    private readonly CancellationToken _cancellationToken;

    public WindowedAsyncEnumerable(
        IAsyncEnumerable<SensorMeasurement<NetvoxMeasurement>> source,
        MeasurementWindow window,
        CancellationToken cancellationToken = default)
    {
        _source = source;
        _window = window;
        _cancellationToken = cancellationToken;
    }

    public IAsyncEnumerator<object> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        var LinkedCTSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _cancellationToken);

        var windowedValues = _window.Type switch
        {
            MeasurementWindowType.Integral => _source.Window(Math.Max(1, Math.Min(60, _window.WindowSize)), LinkedCTSource.Token),
            _ => _source.Window(CreateDurationWindow(TimeSpan.FromMinutes(1).Max(TimeSpan.FromHours(6).Min(_window.WindowDuration))), LinkedCTSource.Token)
        };

        return windowedValues.GetAsyncEnumerator(LinkedCTSource.Token)
            .Transform(static batch => (object)batch.Aggregate(Aggregate), LinkedCTSource.Token);


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
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MongoDB.Driver;

namespace Sensify.Extensions;

public static class AsyncEnumerableExtensions
{

    public static async IAsyncEnumerator<K> GetAsyncEnumerator<T,K>(this IFindFluent<T, K> findFluent, CancellationToken cancellationToken = default)
    {
        var cursor = await findFluent.ToCursorAsync(cancellationToken);

        while(await cursor.MoveNextAsync(cancellationToken))
        {
            foreach(var item in cursor.Current)
            {
                yield return item;
            }
        }
    }

    public static async IAsyncEnumerable<K> GetAsyncEnumerable<T,K>(this IFindFluent<T, K> findFluent, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var cursor = await findFluent.ToCursorAsync(cancellationToken);

        while(await cursor.MoveNextAsync(cancellationToken))
        {
            foreach(var item in cursor.Current)
            {
                yield return item;
            }
        }
    }

    public static IAsyncEnumerable<T[]> Window<T, TWindowSize>(
        this IAsyncEnumerable<T> asyncEnumerable,
        TWindowSize windowSize,
        CancellationToken cancellationToken = default)
        where TWindowSize
        : IIncrementOperators<TWindowSize>,
        IAdditiveIdentity<TWindowSize, TWindowSize>,
        IComparisonOperators<TWindowSize, TWindowSize, bool>
        => Window(asyncEnumerable.GetAsyncEnumerator(cancellationToken), windowSize, cancellationToken);

    public static async IAsyncEnumerable<T[]> Window<T, TWindowSize>(
        this IAsyncEnumerator<T> asyncEnumerator,
        TWindowSize windowSize,
        [EnumeratorCancellation] CancellationToken cancellationToken = default) 
        where TWindowSize 
        : IIncrementOperators<TWindowSize>,
        IAdditiveIdentity<TWindowSize, TWindowSize>,
        IComparisonOperators<TWindowSize, TWindowSize, bool>
    {
        TWindowSize count = TWindowSize.AdditiveIdentity;

        List<T> values = [];

        while(await asyncEnumerator.MoveNextAsync())
        {
            cancellationToken.ThrowIfCancellationRequested();
            values.Add(asyncEnumerator.Current);
            count++;

            if (count >= windowSize)
            {
                count = TWindowSize.AdditiveIdentity;
                yield return values.ToArray();
                CollectionsMarshal.SetCount(values, 0);
            }
        }

    }

    public static IAsyncEnumerable<T[]> Window<T>(
        this IAsyncEnumerable<T> asyncEnumerable,
        Func<T?, T, IReadOnlyList<T>, bool> windowBreak,
        CancellationToken cancellationToken = default)
         => Window(asyncEnumerable.GetAsyncEnumerator(cancellationToken), windowBreak, cancellationToken);

    public static async IAsyncEnumerable<T[]> Window<T>(
        this IAsyncEnumerator<T> asyncEnumerator,
        Func<T?,T, IReadOnlyList<T>,bool> windowBreak,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {

        List<T> values = [];

        while(await asyncEnumerator.MoveNextAsync())
        {
            cancellationToken.ThrowIfCancellationRequested();
            var current = asyncEnumerator.Current;
            T? windowStart = values.Count == 0 ? default : current;


            if (windowBreak(windowStart, current, values))
            {
                yield return values.ToArray();
                CollectionsMarshal.SetCount(values, 0);
            }
            values.Add(current);
        }

    }

    public static async IAsyncEnumerator<R> Transform<T,R>(this IAsyncEnumerator<T> asyncEnumerator, Func<T, R> transform, CancellationToken cancellationToken = default)
    {
        while(await asyncEnumerator.MoveNextAsync())
        {
            yield return transform(asyncEnumerator.Current);
        }
    }
    public static async IAsyncEnumerable<R> Transform<T,R>(this IAsyncEnumerable<T> asyncEnumerable, Func<T, R> transform, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var value in asyncEnumerable)
        {
            yield return transform(value);
        }
    }

}
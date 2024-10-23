﻿using System.Numerics;
using System.Runtime.InteropServices;
using Sensify.Decoders.Common;

namespace Sensify.Extensions;

public static class GenericMeasurementExtensions
{
    public static GenericMeasurement<T>? Add<T>(this GenericMeasurement<T>? lhs, GenericMeasurement<T>? rhs) where T : IAdditionOperators<T, T, T>
    {
        if (lhs is not null && rhs is not null)
        {
            if (lhs.Unit == rhs.Unit) return lhs with { Value = lhs.Value + rhs.Value };

            return lhs with { Value = lhs.Value + rhs.Value, Unit = MeasurementUnit.None };
        }

        return lhs is null ? rhs : lhs;

    }
    
    public static GenericMeasurement<bool>? Add(this GenericMeasurement<bool>? lhs, GenericMeasurement<bool>? rhs)
    {
        if (lhs is not null && rhs is not null)
        {
            if (lhs.Unit == rhs.Unit) return lhs with { Value = lhs.Value | rhs.Value };

            return lhs with { Value = lhs.Value | rhs.Value, Unit = MeasurementUnit.None };
        }

        return lhs is null ? rhs : lhs;

    }   

    public static GenericMeasurement<T[]>? Add<T>(this GenericMeasurement<T[]>? lhs, GenericMeasurement<T[]>? rhs) where T : IAdditionOperators<T, T, T>
    {
        if (lhs is not null && rhs is not null)
        {
            if (lhs.Unit == rhs.Unit) return new(Add(lhs.Value, rhs.Value),  lhs.Unit);

            return lhs with { Value = Add(lhs.Value, rhs.Value), Unit = MeasurementUnit.None };
        }

        return lhs is null ? rhs : lhs;

    }   
    public static GenericMeasurement<List<T>>? Add<T>(this GenericMeasurement<List<T>>? lhs, GenericMeasurement<List<T>>? rhs) where T : IAdditionOperators<T, T, T>
    {
        if (lhs is not null && rhs is not null)
        {
            if (lhs.Unit == rhs.Unit) return new(Add(lhs.Value, rhs.Value));

            return lhs with { Value = Add(lhs.Value, rhs.Value), Unit = MeasurementUnit.None };
        }


        return lhs is null ? rhs : lhs;


    } 
    
    public static GenericMeasurement<List<Tuple<T,K>>>? Add<T,K>(this GenericMeasurement<List<Tuple<T,K>>>? lhs, GenericMeasurement<List<Tuple<T,K>>>? rhs) where T : IAdditionOperators<T, T, T> where K: IAdditionOperators<K, K, K>
    {
        if (lhs is not null && rhs is not null)
        {
            if (lhs.Unit == rhs.Unit) return new(Add(lhs.Value, rhs.Value));

            return lhs with { Value = Add(lhs.Value, rhs.Value), Unit = MeasurementUnit.None };
        }


        return lhs is null ? rhs : lhs;


    }

    private static Span<T> Add<T>(Span<T> left, Span<T> right) where T : IAdditionOperators<T, T, T>
    {

        if (left.Length > right.Length)
        {
            for (int i = 0; i < right.Length; i++)
            {
                left[i] += right[i];
            }

            return left;
        }

        for (int i = 0; i < left.Length; i++)
        {
            right[i] += left[i];
        }

        return right;
    }

    private static T[] Add<T>(T[] lhs, T[] rhs) where T : IAdditionOperators<T, T, T>
    {
        Span<T> left = lhs;
        Span<T> right = rhs;

        Add(lhs, rhs);

        return left.Length > right.Length ? lhs : rhs;
    }

    private static List<T> Add<T>(List<T> lhs, List<T> rhs) where T : IAdditionOperators<T, T, T>
    {
        Span<T> left = CollectionsMarshal.AsSpan(lhs);
        Span<T> right = CollectionsMarshal.AsSpan(rhs);

        Add(left, right);

        return left.Length > right.Length ? lhs : rhs;
    }

    private static Span<T> Add<T>(Span<T> left, Span<T> right, Func<T, T, T> aggregator)
    {

        if (left.Length > right.Length)
        {
            for (int i = 0; i < right.Length; i++)
            {
                left[i] = aggregator(left[i], right[i]);
            }

            return left;
        }

        for (int i = 0; i < left.Length; i++)
        {
            right[i] = aggregator(right[i], left[i]);
        }

        return right;
    }

    private static List<Tuple<T, K>> Add<T, K>(List<Tuple<T,K>> lhs, List<Tuple<T, K>> rhs) where T : IAdditionOperators<T, T, T> where K : IAdditionOperators<K, K, K>
    {
        Span<Tuple<T, K>> left = CollectionsMarshal.AsSpan(lhs);
        Span<Tuple<T, K>> right = CollectionsMarshal.AsSpan(rhs);

        Add(left, right, static (_lhs, _rhs) => new(_lhs.Item1 + _rhs.Item1, _lhs.Item2 + _rhs.Item2));

        return left.Length > right.Length ? lhs : rhs;
    }
}
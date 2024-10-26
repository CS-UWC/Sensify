using System.Numerics;
using System.Runtime.InteropServices;
using Sensify.Decoders.Common;
using Sensify.Decoders.Elsys;
using Sensify.Decoders.Netvox;
using Sensify.Decoders.Synetica;

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
            if (lhs.Unit == rhs.Unit) return new(Add(lhs.Value, rhs.Value), lhs.Unit);

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

    public static GenericMeasurement<List<Tuple<T, K>>>? Add<T, K>(this GenericMeasurement<List<Tuple<T, K>>>? lhs, GenericMeasurement<List<Tuple<T, K>>>? rhs) where T : IAdditionOperators<T, T, T> where K : IAdditionOperators<K, K, K>
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

    private static List<Tuple<T, K>> Add<T, K>(List<Tuple<T, K>> lhs, List<Tuple<T, K>> rhs) where T : IAdditionOperators<T, T, T> where K : IAdditionOperators<K, K, K>
    {
        Span<Tuple<T, K>> left = CollectionsMarshal.AsSpan(lhs);
        Span<Tuple<T, K>> right = CollectionsMarshal.AsSpan(rhs);

        Add(left, right, static (_lhs, _rhs) => new(_lhs.Item1 + _rhs.Item1, _lhs.Item2 + _rhs.Item2));

        return left.Length > right.Length ? lhs : rhs;
    }

    public static GenericMeasurement<T>? Sub<T>(this GenericMeasurement<T>? lhs, GenericMeasurement<T>? rhs) where T : ISubtractionOperators<T, T, T>
    {
        if (lhs is not null && rhs is not null)
        {
            if (lhs.Unit == rhs.Unit) return lhs with { Value = lhs.Value - rhs.Value };

            return lhs with { Value = lhs.Value - rhs.Value, Unit = MeasurementUnit.None };
        }

        return lhs is null ? rhs : lhs;

    }

    public static GenericMeasurement<T[]>? Sub<T>(this GenericMeasurement<T[]>? lhs, GenericMeasurement<T[]>? rhs) where T : ISubtractionOperators<T, T, T>
    {
        if (lhs is not null && rhs is not null)
        {
            if (lhs.Unit == rhs.Unit) return new(Sub(lhs.Value, rhs.Value), lhs.Unit);

            return lhs with { Value = Sub(lhs.Value, rhs.Value), Unit = MeasurementUnit.None };
        }

        return lhs is null ? rhs : lhs;

    }
    public static GenericMeasurement<List<T>>? Sub<T>(this GenericMeasurement<List<T>>? lhs, GenericMeasurement<List<T>>? rhs) where T : ISubtractionOperators<T, T, T>
    {
        if (lhs is not null && rhs is not null)
        {
            if (lhs.Unit == rhs.Unit) return new(Sub(lhs.Value, rhs.Value));

            return lhs with { Value = Sub(lhs.Value, rhs.Value), Unit = MeasurementUnit.None };
        }


        return lhs is null ? rhs : lhs;


    }

    public static GenericMeasurement<List<Tuple<T, K>>>? Sub<T, K>(this GenericMeasurement<List<Tuple<T, K>>>? lhs, GenericMeasurement<List<Tuple<T, K>>>? rhs) 
        where T : ISubtractionOperators<T, T, T> 
        where K : ISubtractionOperators<K, K, K>
    {
        if (lhs is not null && rhs is not null)
        {
            if (lhs.Unit == rhs.Unit) return new(Sub(lhs.Value, rhs.Value));

            return lhs with { Value = Sub(lhs.Value, rhs.Value), Unit = MeasurementUnit.None };
        }


        return lhs is null ? rhs : lhs;


    }

    private static Span<T> Sub<T>(Span<T> left, Span<T> right) where T : ISubtractionOperators<T, T, T>
    {

        if (left.Length > right.Length)
        {
            for (int i = 0; i < right.Length; i++)
            {
                left[i] -= right[i];
            }

            return left;
        }

        for (int i = 0; i < left.Length; i++)
        {
            right[i] -= left[i];
        }

        return right;
    }

    private static T[] Sub<T>(T[] lhs, T[] rhs) where T : ISubtractionOperators<T, T, T>
    {
        Span<T> left = lhs;
        Span<T> right = rhs;

        Sub(lhs, rhs);

        return left.Length > right.Length ? lhs : rhs;
    }

    private static List<T> Sub<T>(List<T> lhs, List<T> rhs) where T : ISubtractionOperators<T, T, T>
    {
        Span<T> left = CollectionsMarshal.AsSpan(lhs);
        Span<T> right = CollectionsMarshal.AsSpan(rhs);

        Sub(left, right);

        return left.Length > right.Length ? lhs : rhs;
    }

    private static Span<T> Sub<T>(Span<T> left, Span<T> right, Func<T, T, T> aggregator)
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

    private static List<Tuple<T, K>> Sub<T, K>(List<Tuple<T, K>> lhs, List<Tuple<T, K>> rhs) where T : ISubtractionOperators<T, T, T> where K : ISubtractionOperators<K, K, K>
    {
        Span<Tuple<T, K>> left = CollectionsMarshal.AsSpan(lhs);
        Span<Tuple<T, K>> right = CollectionsMarshal.AsSpan(rhs);

        Sub(left, right, static (_lhs, _rhs) => new(_lhs.Item1 - _rhs.Item1, _lhs.Item2 - _rhs.Item2));

        return left.Length > right.Length ? lhs : rhs;
    }

    public static GenericMeasurement<K>? Div<T, K>(this GenericMeasurement<T>? measurement, K value) where T : IDivisionOperators<T, K, K> where K : INumberBase<K>
    {
        if (measurement is null) return null;

        return new(measurement.Value / value, measurement.Unit);
    }

    public static GenericMeasurement<T>? DivSame<T, K>(this GenericMeasurement<T>? measurement, K value) where T : IDivisionOperators<T, K, T> where K : INumberBase<K>
    {
        if (measurement is null) return null;

        return new(measurement.Value / value, measurement.Unit);
    }

    public static GenericMeasurement<K>? Mul<T, K>(this GenericMeasurement<T>? measurement, K value) where T : IMultiplyOperators<T, K, K> where K : INumberBase<K>
    {
        if (measurement is null) return null;

        return new(measurement.Value * value, measurement.Unit);
    }

    public static GenericMeasurement<T>? Mul<T>(this GenericMeasurement<T>? lhs, GenericMeasurement<T>? rhs) where T : IMultiplyOperators<T, T, T>
    {

        if (lhs is null) return rhs;
        if (rhs is null) return lhs;

        return new(lhs.Value * rhs.Value, lhs.Unit);
    }

    public static GenericMeasurement<T>? Max<T>(this GenericMeasurement<T>? lhs, GenericMeasurement<T> ?rhs) where T : IComparisonOperators<T, T, bool>
    {

        if (lhs is null) return rhs;
        if (rhs is null) return lhs;

        return new(lhs.Value > rhs.Value ? lhs.Value : rhs.Value, lhs.Unit);
    }

    public static GenericMeasurement<T>? Min<T>(this GenericMeasurement<T>? lhs, GenericMeasurement<T>? rhs) where T : IComparisonOperators<T, T, bool>
    {
        if (lhs is null) return rhs;
        if (rhs is null) return lhs;

        return new(lhs.Value < rhs.Value ? lhs.Value : rhs.Value, lhs.Unit);
    }

    public static GenericMeasurement<double>? Sqrt(this GenericMeasurement<double>? measurement)
    {
        if (measurement is null) return null;

        return new( Math.Sqrt(measurement.Value), measurement.Unit);
    }

    public static GenericMeasurement<Vector3<T>>? Min<T>(this GenericMeasurement<Vector3<T>>? lhs,  GenericMeasurement<Vector3<T>>? rhs) 
        where T : IComparisonOperators<T, T, bool>,
        IAdditionOperators<T, T, T>,
        IAdditiveIdentity<T, T>,
        ISubtractionOperators<T, T, T>,
        IMultiplicativeIdentity<T, T>,
        IMultiplyOperators<T, T, T>,
        IDivisionOperators<T, T, T>
    {
        if (lhs is null) return rhs;
        if (rhs is null) return lhs;


        var lhs_size = lhs.Value.X * lhs.Value.X + lhs.Value.Y * lhs.Value.Y + lhs.Value.Z * lhs.Value.Z;
        var rhs_size = rhs.Value.X * rhs.Value.X + rhs.Value.Y * rhs.Value.Y + rhs.Value.Z * rhs.Value.Z;

        if (lhs_size <= rhs_size) return lhs;

        return rhs;
    }

    public static GenericMeasurement<Vector3<T>>? Max<T>(this GenericMeasurement<Vector3<T>>? lhs,  GenericMeasurement<Vector3<T>>? rhs) 
        where T : IComparisonOperators<T, T, bool>,
        IAdditionOperators<T, T, T>,
        IAdditiveIdentity<T, T>,
        ISubtractionOperators<T, T, T>,
        IMultiplicativeIdentity<T, T>,
        IMultiplyOperators<T, T, T>,
        IDivisionOperators<T, T, T>
    {
        if (lhs is null) return rhs;
        if (rhs is null) return lhs;


        var lhs_size = lhs.Value.X * lhs.Value.X + lhs.Value.Y * lhs.Value.Y + lhs.Value.Z * lhs.Value.Z;
        var rhs_size = rhs.Value.X * rhs.Value.X + rhs.Value.Y * rhs.Value.Y + rhs.Value.Z * rhs.Value.Z;

        if (lhs_size > rhs_size) return lhs;

        return rhs;
    }


    public static ElsysMeasurementMetric? Min(this ElsysMeasurementMetric? lhs, ElsysMeasurementMetric? rhs)
    {
        if (lhs is null) return rhs;
        if (rhs is null) return lhs;

        return new ElsysMeasurementMetric
        {
            Temperature = lhs.Temperature.Min(rhs.Temperature),
            Humidity = lhs.Humidity.Min(rhs.Humidity),
            Light = lhs.Light.Min(rhs.Light),
            Motion = lhs.Motion.Min(rhs.Motion),
            Co2 = lhs.Co2.Min(rhs.Co2),
            Vdd = lhs.Vdd.Min(rhs.Vdd),
            Pulse1Absolute = lhs.Pulse1Absolute.Min(rhs.Pulse1Absolute),
            Digital = lhs.Digital.Min(rhs.Digital),
            AccelerationMotion = lhs.AccelerationMotion.Min(rhs.AccelerationMotion)
        };
    }

    public static ElsysMeasurementMetric? Max(this ElsysMeasurementMetric? lhs, ElsysMeasurementMetric? rhs)
    {
        if (lhs is null) return rhs;
        if (rhs is null) return lhs;

        return new ElsysMeasurementMetric
        {
            Temperature = lhs.Temperature.Max(rhs.Temperature),
            Humidity = lhs.Humidity.Max(rhs.Humidity),
            Light = lhs.Light.Max(rhs.Light),
            Motion = lhs.Motion.Max(rhs.Motion),
            Co2 = lhs.Co2.Max(rhs.Co2),
            Vdd = lhs.Vdd.Max(rhs.Vdd),
            Pulse1Absolute = lhs.Pulse1Absolute.Max(rhs.Pulse1Absolute),
            Digital = lhs.Digital.Max(rhs.Digital),
            AccelerationMotion = lhs.AccelerationMotion.Max(rhs.AccelerationMotion)
        };
    }

    public static ElsysMeasurementMetric? Sqrt(this ElsysMeasurementMetric? measurementMetric)
    {
        if (measurementMetric is null) return null;

        return new ElsysMeasurementMetric
        {
            Temperature = measurementMetric.Temperature.Sqrt(),
            Humidity = measurementMetric.Humidity.Sqrt(),
            Light = measurementMetric.Light.Sqrt(),
            Motion = measurementMetric.Motion.Sqrt(),
            Co2 = measurementMetric.Co2.Sqrt(),
            Vdd = measurementMetric.Vdd.Sqrt(),
            Pulse1Absolute = measurementMetric.Pulse1Absolute.Sqrt(),
            Digital = measurementMetric.Digital.Sqrt(),
            AccelerationMotion = measurementMetric.AccelerationMotion.Sqrt()
        };
    }
    
    public static SyneticaMeasurementMetric? Min(this SyneticaMeasurementMetric? lhs, SyneticaMeasurementMetric? rhs)
    {
        if (lhs is null) return rhs;
        if (rhs is null) return lhs;

        return new SyneticaMeasurementMetric
        {
            Temperature = lhs.Temperature.Min(rhs.Temperature),
            Humidity = lhs.Humidity.Min(rhs.Humidity),
            AmbientLight = lhs.AmbientLight.Min(rhs.AmbientLight),
            Pressure = lhs.Pressure.Min(rhs.Pressure),
            VolatileOrganicCompounds = lhs.VolatileOrganicCompounds.Min(rhs.VolatileOrganicCompounds),
            Bvoc = lhs.Bvoc.Min(rhs.Bvoc),
            Co2e = lhs.Co2e.Min(rhs.Co2e),
            SoundMin = lhs.SoundMin.Min(rhs.SoundMin),
            SoundAvg = lhs.SoundAvg.Min(rhs.SoundAvg),
            SoundMax = lhs.SoundMax.Min(rhs.SoundMax),
            BattVolt = lhs.BattVolt.Min(rhs.BattVolt),
        };
    }

    public static SyneticaMeasurementMetric? Max(this SyneticaMeasurementMetric? lhs, SyneticaMeasurementMetric? rhs)
    {
        if (lhs is null) return rhs;
        if (rhs is null) return lhs;

        return new SyneticaMeasurementMetric
        {
            Temperature = lhs.Temperature.Max(rhs.Temperature),
            Humidity = lhs.Humidity.Max(rhs.Humidity),
            AmbientLight = lhs.AmbientLight.Max(rhs.AmbientLight),
            Pressure = lhs.Pressure.Max(rhs.Pressure),
            VolatileOrganicCompounds = lhs.VolatileOrganicCompounds.Max(rhs.VolatileOrganicCompounds),
            Bvoc = lhs.Bvoc.Max(rhs.Bvoc),
            Co2e = lhs.Co2e.Max(rhs.Co2e),
            SoundMin = lhs.SoundMin.Max(rhs.SoundMin),
            SoundAvg = lhs.SoundAvg.Max(rhs.SoundAvg),
            SoundMax = lhs.SoundMax.Max(rhs.SoundMax),
            BattVolt = lhs.BattVolt.Max(rhs.BattVolt),
        };
    }

    public static SyneticaMeasurementMetric? Sqrt(this SyneticaMeasurementMetric? measurementMetric)
    {
        if (measurementMetric is null) return null;

        return new SyneticaMeasurementMetric
        {
            Temperature = measurementMetric.Temperature.Sqrt(),
            Humidity = measurementMetric.Humidity.Sqrt(),
            AmbientLight = measurementMetric.AmbientLight.Sqrt(),
            Pressure = measurementMetric.Pressure.Sqrt(),
            VolatileOrganicCompounds = measurementMetric.VolatileOrganicCompounds.Sqrt(),
            Bvoc = measurementMetric.Bvoc.Sqrt(),
            Co2e = measurementMetric.Co2e.Sqrt(),
            SoundMin = measurementMetric.SoundMin.Sqrt(),
            SoundAvg = measurementMetric.SoundAvg.Sqrt(),
            SoundMax = measurementMetric.SoundMax.Sqrt(),
            BattVolt = measurementMetric.BattVolt.Sqrt(),
        };
    }

    public static NetvoxMeasurementMetric? Min(this NetvoxMeasurementMetric? lhs, NetvoxMeasurementMetric? rhs)
    {
        if (lhs is null) return rhs;
        if (rhs is null) return lhs;

        return new NetvoxMeasurementMetric
        {
            Battery = lhs.Battery.Min(rhs.Battery),
            Temperature = lhs.Temperature.Min(rhs.Temperature),
            Temperature1 = lhs.Temperature1.Min(rhs.Temperature1),
            Temperature2 = lhs.Temperature2.Min(rhs.Temperature2),
            Temperature3 = lhs.Temperature3.Min(rhs.Temperature3),
        };
    }

    public static NetvoxMeasurementMetric? Max(this NetvoxMeasurementMetric? lhs, NetvoxMeasurementMetric? rhs)
    {
        if (lhs is null) return rhs;
        if (rhs is null) return lhs;

        return new NetvoxMeasurementMetric
        {
            Battery = lhs.Battery.Max(rhs.Battery),
            Temperature = lhs.Temperature.Max(rhs.Temperature),
            Temperature1 = lhs.Temperature1.Max(rhs.Temperature1),
            Temperature2 = lhs.Temperature2.Max(rhs.Temperature2),
            Temperature3 = lhs.Temperature3.Max(rhs.Temperature3),
        };
    }

    public static NetvoxMeasurementMetric? Sqrt(this NetvoxMeasurementMetric? measurementMetric)
    {
        if (measurementMetric is null) return null;

        return new NetvoxMeasurementMetric
        {
            Battery = measurementMetric.Battery.Sqrt(),
            Temperature = measurementMetric.Temperature.Sqrt(),
            Temperature1 = measurementMetric.Temperature1.Sqrt(),
            Temperature2 = measurementMetric.Temperature2.Sqrt(),
            Temperature3 = measurementMetric.Temperature3.Sqrt(),
        };
    }

}
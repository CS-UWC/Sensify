using Sensify.Decoders.Common;
using Sensify.Extensions;

namespace Sensify.Decoders.Elsys;

[GenerateSerializer]
[Alias("Sensify.Decoders.Elsys.ElsysMeasurementMetric")]
public partial record ElsysMeasurementMetric : IMetric
{
    [Id(0)]
    public GenericMeasurement<double>? Temperature { get; set; }
    [Id(1)]
    public GenericMeasurement<double>? Humidity { get; set; }
    [Id(2)]
    public GenericMeasurement<double>? Light { get; set; }
    [Id(3)]
    public GenericMeasurement<double>? Motion { get; set; }
    [Id(4)]
    public GenericMeasurement<double>? Co2 { get; set; }
    [Id(5)]
    public GenericMeasurement<double>? Vdd { get; set; }
    [Id(6)]
    public GenericMeasurement<double>? Pulse1Absolute { get; set; }
    [Id(7)]
    public GenericMeasurement<double>? Digital { get; set; }
    [Id(8)]
    public GenericMeasurement<double>? AccelerationMotion { get; set; }

    public static ElsysMeasurementMetric? operator /(ElsysMeasurementMetric? lhs, double rhs)
    {
        if (lhs is null) return null;

        return new ElsysMeasurementMetric
        {
            Temperature = lhs is null ? default : lhs.Temperature.Div(rhs),
            Humidity = lhs is null ? default : lhs.Humidity.Div(rhs),
            Light = lhs is null ? default : lhs.Light.Div(rhs),
            Motion = lhs is null ? default : lhs.Motion.Div(rhs),
            Co2 = lhs is null ? default : lhs.Co2.Div(rhs),
            Vdd = lhs is null ? default : lhs.Vdd.Div(rhs),
            Pulse1Absolute = lhs is null ? default : lhs.Pulse1Absolute.Div(rhs),
            Digital = lhs is null ? default : lhs.Digital.Div(rhs),
            AccelerationMotion = lhs is null ? default : lhs.AccelerationMotion.Div(rhs)
        };
    }

    public static ElsysMeasurementMetric? operator *(ElsysMeasurementMetric? lhs, ElsysMeasurementMetric? rhs)
    {
        if (lhs is null) return rhs;
        if (rhs is null) return lhs;

        return new ElsysMeasurementMetric
        {
            Temperature = lhs.Temperature.Mul(rhs.Temperature),
            Humidity = lhs.Humidity.Mul(rhs.Humidity),
            Light = lhs.Light.Mul(rhs.Light),
            Motion = lhs.Motion.Mul(rhs.Motion),
            Co2 = lhs.Co2.Mul(rhs.Co2),
            Vdd = lhs.Vdd.Mul(rhs.Vdd),
            Pulse1Absolute = lhs.Pulse1Absolute.Mul(rhs.Pulse1Absolute),
            Digital = lhs.Digital.Mul(rhs.Digital),
            AccelerationMotion = lhs.AccelerationMotion.Mul(rhs.AccelerationMotion)
        };
    }

    public static ElsysMeasurementMetric? operator +(ElsysMeasurementMetric? lhs, ElsysMeasurementMetric? rhs)
    {
        if(lhs is null) return rhs;
        if(rhs is null) return lhs;

        return new ElsysMeasurementMetric
        {
            Temperature = lhs.Temperature.Add(rhs.Temperature),
            Humidity = lhs.Humidity.Add(rhs.Humidity),
            Light = lhs.Light.Add(rhs.Light),
            Motion = lhs.Motion.Add(rhs.Motion),
            Co2 = lhs.Co2.Add(rhs.Co2),
            Vdd = lhs.Vdd.Add(rhs.Vdd),
            Pulse1Absolute = lhs.Pulse1Absolute.Add(rhs.Pulse1Absolute),
            Digital = lhs.Digital.Add(rhs.Digital),
            AccelerationMotion = lhs.AccelerationMotion.Add(rhs.AccelerationMotion)
        };
    }

    public static ElsysMeasurementMetric? operator -(ElsysMeasurementMetric? lhs, ElsysMeasurementMetric? rhs)
    {
        if(lhs is null) return rhs;
        if(rhs is null) return lhs;

        return new ElsysMeasurementMetric
        {
            Temperature = lhs.Temperature.Sub(rhs.Temperature),
            Humidity = lhs.Humidity.Sub(rhs.Humidity),
            Light = lhs.Light.Sub(rhs.Light),
            Motion = lhs.Motion.Sub(rhs.Motion),
            Co2 = lhs.Co2.Sub(rhs.Co2),
            Vdd = lhs.Vdd.Sub(rhs.Vdd),
            Pulse1Absolute = lhs.Pulse1Absolute.Sub(rhs.Pulse1Absolute),
            Digital = lhs.Digital.Sub(rhs.Digital),
            AccelerationMotion = lhs.AccelerationMotion.Sub(rhs.AccelerationMotion)
        };
    }

}
using Sensify.Decoders.Common;
using Sensify.Extensions;

namespace Sensify.Decoders.Netvox;

[GenerateSerializer]
[Alias("Sensify.Decoders.Netvox.NetvoxMeasurementMetric")]
public partial record NetvoxMeasurementMetric
{
    [Id(0)]
    public GenericMeasurement<double>? Battery { get; set; }
    [Id(1)]
    public GenericMeasurement<double>? Temperature { get; set; }
    [Id(2)]
    public GenericMeasurement<double>? Temperature1 { get; set; }
    [Id(3)]
    public GenericMeasurement<double>? Temperature2 { get; set; }
    [Id(4)]
    public GenericMeasurement<double>? Temperature3 { get; set; }

    public static NetvoxMeasurementMetric? operator /(NetvoxMeasurementMetric? lhs, double rhs)
    {
        if (lhs is null) return null;

        return new NetvoxMeasurementMetric
        {
            Battery = lhs is null ? default : lhs.Battery.Div(rhs),
            Temperature = lhs is null ? default : lhs.Temperature.Div(rhs),
            Temperature1 = lhs is null ? default : lhs.Temperature1.Div(rhs),
            Temperature2 = lhs is null ? default : lhs.Temperature2.Div(rhs),
            Temperature3 = lhs is null ? default : lhs.Temperature3.Div(rhs)
        };
    }

    public static NetvoxMeasurementMetric? operator *(NetvoxMeasurementMetric? lhs, NetvoxMeasurementMetric? rhs)
    {
        if (lhs is null) return rhs;
        if (rhs is null) return lhs;

        return new NetvoxMeasurementMetric
        {
            Battery = lhs.Battery.Mul(rhs.Battery),
            Temperature = lhs.Temperature.Mul(rhs.Temperature),
            Temperature1 = lhs.Temperature1.Mul(rhs.Temperature1),
            Temperature2 = lhs.Temperature2.Mul(rhs.Temperature2),
            Temperature3 = lhs.Temperature3.Mul(rhs.Temperature3)
        };
    }

    public static NetvoxMeasurementMetric? operator +(NetvoxMeasurementMetric? lhs, NetvoxMeasurementMetric? rhs)
    {
        if(lhs is null) return rhs;
        if(rhs is null) return lhs;

        return new NetvoxMeasurementMetric
        {
            Battery = lhs.Battery.Add(rhs.Battery),
            Temperature = lhs.Temperature.Add(rhs.Temperature),
            Temperature1 = lhs.Temperature1.Add(rhs.Temperature1),
            Temperature2 = lhs.Temperature2.Add(rhs.Temperature2),
            Temperature3 = lhs.Temperature3.Add(rhs.Temperature3)
        };
    }

    public static NetvoxMeasurementMetric? operator -(NetvoxMeasurementMetric? lhs, NetvoxMeasurementMetric? rhs)
    {
        if(lhs is null) return rhs;
        if(rhs is null) return lhs;

        return new NetvoxMeasurementMetric
        {
            Battery = lhs.Battery.Sub(rhs.Battery),
            Temperature = lhs.Temperature.Sub(rhs.Temperature),
            Temperature1 = lhs.Temperature1.Sub(rhs.Temperature1),
            Temperature2 = lhs.Temperature2.Sub(rhs.Temperature2),
            Temperature3 = lhs.Temperature3.Sub(rhs.Temperature3)
        };
    }

}
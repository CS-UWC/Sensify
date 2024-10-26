using Sensify.Extensions;

namespace Sensify.Decoders.Netvox;
public partial record NetvoxMeasurement
{
    public static NetvoxMeasurement operator +(NetvoxMeasurement lhs, NetvoxMeasurement rhs)
    {
        return new NetvoxMeasurement
        {
            Battery = lhs is null ? rhs?.Battery : lhs.Battery.Add(rhs?.Temperature),
            Acceleration = lhs is null ? rhs?.Acceleration : lhs.Acceleration.Add(rhs?.Acceleration),
            Velocity = lhs is null ? rhs?.Velocity : lhs.Velocity.Add(rhs?.Velocity),
            Temperature = lhs is null ? rhs?.Temperature : lhs.Temperature.Add(rhs?.Temperature),
            Temperature1 = lhs is null ? rhs?.Temperature1 : lhs.Temperature1.Add(rhs?.Temperature1),
            Temperature2 = lhs is null ? rhs?.Temperature2 : lhs.Temperature2.Add(rhs?.Temperature2),
            Temperature3 = lhs is null ? rhs?.Temperature3 : lhs.Temperature3.Add(rhs?.Temperature3),
        };

    }


    public static NetvoxMeasurementMetric? operator /(NetvoxMeasurement? lhs, double rhs)
    {
        NetvoxMeasurementMetric? metric = lhs;
        if (metric is null) return null;

        return new NetvoxMeasurementMetric
        {
            Battery = metric is null ? default : metric.Battery.Div(rhs),
            Temperature = metric is null ? default : metric.Temperature.Div(rhs),
            Temperature1 = metric is null ? default : metric.Temperature1.Div(rhs),
            Temperature2 = metric is null ? default : metric.Temperature2.Div(rhs),
            Temperature3 = metric is null ? default : metric.Temperature3.Div(rhs),
        };
    }

    public static implicit operator NetvoxMeasurementMetric?(NetvoxMeasurement? measurement)
    {
        if (measurement is null) return null;

        return new NetvoxMeasurementMetric
        {
            Battery = measurement?.Battery is null ? default : new(measurement.Battery.Value, measurement.Battery.Unit),
            Temperature = measurement?.Temperature is null ? default : new(measurement.Temperature.Value, measurement.Temperature.Unit),
            Temperature1 = measurement?.Temperature1 is null ? default : new(measurement.Temperature1.Value, measurement.Temperature1.Unit),
            Temperature2 = measurement?.Temperature2 is null ? default : new(measurement.Temperature2.Value, measurement.Temperature2.Unit),
            Temperature3 = measurement?.Temperature3 is null ? default : new(measurement.Temperature3.Value, measurement.Temperature3.Unit)
        };

    }
}

using Sensify.Extensions;

namespace Sensify.Decoders.Elsys;
public partial record ElsysMeasurement
{
    public static ElsysMeasurement operator +(ElsysMeasurement lhs, ElsysMeasurement rhs)
    {
        return new ElsysMeasurement
        {
             Temperature = lhs is null ? rhs?.Temperature : lhs.Temperature.Add(rhs?.Temperature), 
             Humidity = lhs is null ? rhs?.Humidity : lhs.Humidity.Add(rhs?.Humidity),
             Acceleration = lhs is null ? rhs?.Acceleration : lhs.Acceleration.Add(rhs?.Acceleration),
             Light = lhs is null ? rhs?.Light : lhs.Light.Add(rhs?.Light),
             Motion = lhs is null ? rhs?.Motion : lhs.Motion.Add(rhs?.Motion),
             Co2 = lhs is null ? rhs?.Co2 : lhs.Co2.Add(rhs?.Co2),
             Vdd = lhs is null ? rhs?.Vdd : lhs.Vdd.Add(rhs?.Vdd),
             Pulse1Absolute = lhs is null ? rhs?.Pulse1Absolute : lhs.Pulse1Absolute.Add(rhs?.Pulse1Absolute),
             Digital = lhs is null ? rhs?.Digital : lhs.Digital.Add(rhs?.Digital),
             AccelerationMotion = lhs is null ? rhs?.AccelerationMotion : lhs.AccelerationMotion.Add(rhs?.AccelerationMotion)
        };

    }

    public static ElsysMeasurementMetric? operator /(ElsysMeasurement? lhs, double rhs)
    {
        ElsysMeasurementMetric? metric = lhs;
        if (metric is null) return null;

        return new ElsysMeasurementMetric
        {
            Temperature = metric is null ? default : metric.Temperature.Div(rhs),
            Humidity = metric is null ? default: metric.Humidity.Div(rhs),
            Light = metric is null ? default: metric.Light.Div(rhs),
            Motion = metric is null ? default: metric.Motion.Div(rhs),
            Co2 = metric is null ? default: metric.Co2.Div(rhs),
            Vdd = metric is null ? default: metric.Vdd.Div(rhs),
            Pulse1Absolute = metric is null ? default: metric.Pulse1Absolute.Div(rhs),
            Digital = metric is null ? default: metric.Digital.Div(rhs),
            AccelerationMotion = metric is null ? default: metric.AccelerationMotion.Div(rhs)
        };
    }

    public static implicit operator ElsysMeasurementMetric?(ElsysMeasurement? measurement)
    {
        if (measurement is null) return null;

        return new ElsysMeasurementMetric
        {
            Temperature = measurement?.Temperature is null ? default : new(measurement.Temperature.Value, measurement.Temperature.Unit),
            Humidity = measurement?.Humidity is null ? default : new(measurement.Humidity.Value, measurement.Humidity.Unit),
            Light = measurement?.Light is null ? default : new(measurement.Light.Value, measurement.Light.Unit),
            Motion = measurement?.Motion is null ? default : new(measurement.Motion.Value, measurement.Motion.Unit),
            Co2 = measurement?.Co2 is null ? default : new(measurement.Co2.Value, measurement.Co2.Unit),
            Vdd = measurement?.Vdd is null ? default : new(measurement.Vdd.Value, measurement.Vdd.Unit),
            Pulse1Absolute = measurement?.Pulse1Absolute is null ? default : new(measurement.Pulse1Absolute.Value, measurement.Pulse1Absolute.Unit),
            Digital = measurement?.Digital is null ? default : new(measurement.Digital.Value, measurement.Digital.Unit),
            AccelerationMotion = measurement?.AccelerationMotion is null ? default : new(measurement.AccelerationMotion.Value, measurement.AccelerationMotion.Unit)
        };

    }
}

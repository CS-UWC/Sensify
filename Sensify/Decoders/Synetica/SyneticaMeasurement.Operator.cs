using Sensify.Extensions;

namespace Sensify.Decoders.Synetica;

public partial record SyneticaMeasurement
{
    public static SyneticaMeasurement operator +(SyneticaMeasurement lhs, SyneticaMeasurement rhs)
    {
        return new()
        {
            Temperature = lhs is null ? rhs?.Temperature : lhs.Temperature?.Add(rhs?.Temperature),
            Humidity = lhs is null ? rhs?.Humidity : lhs.Humidity?.Add(rhs?.Humidity),
            AmbientLight = lhs is null ? rhs?.AmbientLight : lhs.AmbientLight?.Add(rhs?.AmbientLight),
            Pressure = lhs is null ? rhs?.Pressure : lhs.Pressure?.Add(rhs?.Pressure),
            VolatileOrganicCompounds = lhs is null ? rhs?.VolatileOrganicCompounds : lhs.VolatileOrganicCompounds?.Add(rhs?.VolatileOrganicCompounds),
            Bvoc = lhs is null ? rhs?.Bvoc : lhs.Bvoc?.Add(rhs?.Bvoc),
            Co2e = lhs is null ? rhs?.Co2e : lhs.Co2e?.Add(rhs?.Co2e),
            SoundMin = lhs is null ? rhs?.SoundMin : lhs.SoundMin?.Add(rhs?.SoundMin),
            SoundAvg = lhs is null ? rhs?.SoundAvg : lhs.SoundAvg?.Add(rhs?.SoundAvg),
            SoundMax = lhs is null ? rhs?.SoundMax : lhs.SoundMax?.Add(rhs?.SoundMax),
            BattVolt = lhs is null ? rhs?.BattVolt : lhs.BattVolt?.Add(rhs?.BattVolt)
        };
    }

    public static SyneticaMeasurementMetric? operator /(SyneticaMeasurement? lhs, double rhs)
    {
        SyneticaMeasurementMetric? metric = lhs;
        if (metric is null) return null;

        return new SyneticaMeasurementMetric
        {
            Temperature = metric is null ? default : metric.Temperature?.Div(rhs),
            Humidity = metric is null ? default : metric.Humidity?.Div(rhs),
            AmbientLight = metric is null ? default : metric.AmbientLight?.Div(rhs),
            Pressure = metric is null ? default : metric.Pressure?.Div(rhs),
            VolatileOrganicCompounds = metric is null ? default : metric.VolatileOrganicCompounds?.Div(rhs),
            Bvoc = metric is null ? default : metric.Bvoc?.Div(rhs),
            Co2e = metric is null ? default : metric.Co2e?.Div(rhs),
            SoundMin = metric is null ? default : metric.SoundMin?.Div(rhs),
            SoundAvg = metric is null ? default : metric.SoundAvg?.Div(rhs),
            SoundMax = metric is null ? default : metric.SoundMax?.Div(rhs),
            BattVolt = metric is null ? default : metric.BattVolt?.Div(rhs)
        };
    }

    public static implicit operator SyneticaMeasurementMetric?(SyneticaMeasurement? measurement)
    {
        if (measurement is null) return null;

        return new SyneticaMeasurementMetric
        {
            Temperature = measurement?.Temperature is null ? default : new(measurement.Temperature.Value, measurement.Temperature.Unit),
            Humidity = measurement?.Humidity is null ? default : new(measurement.Humidity.Value, measurement.Humidity.Unit),
            AmbientLight = measurement?.AmbientLight is null ? default : new(measurement.AmbientLight.Value, measurement.AmbientLight.Unit),
            Pressure = measurement?.Pressure is null ? default : new(measurement.Pressure.Value, measurement.Pressure.Unit),
            VolatileOrganicCompounds = measurement?.VolatileOrganicCompounds is null ? default : new(measurement.VolatileOrganicCompounds.Value, measurement.VolatileOrganicCompounds.Unit),
            Bvoc = measurement?.Bvoc is null ? default : new(measurement.Bvoc.Value, measurement.Bvoc.Unit),
            Co2e = measurement?.Co2e is null ? default : new(measurement.Co2e.Value, measurement.Co2e.Unit),
            SoundMin = measurement?.SoundMin is null ? default : new(measurement.SoundMin.Value, measurement.SoundMin.Unit),
            SoundAvg = measurement?.SoundAvg is null ? default : new(measurement.SoundAvg.Value, measurement.SoundAvg.Unit),
            SoundMax = measurement?.SoundMax is null ? default : new(measurement.SoundMax.Value, measurement.SoundMax.Unit),
            BattVolt = measurement?.BattVolt is null ? default : new(measurement.BattVolt.Value, measurement.BattVolt.Unit)
        };

    }

}
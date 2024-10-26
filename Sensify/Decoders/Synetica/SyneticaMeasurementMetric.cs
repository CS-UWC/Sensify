using Sensify.Decoders.Common;
using Sensify.Extensions;

namespace Sensify.Decoders.Synetica;

[GenerateSerializer]
[Alias("Sensify.Decoders.Synetica.SyneticaMeasurementMetric")]
public partial record SyneticaMeasurementMetric
{
    [Id(0)]
    public GenericMeasurement<double>? Temperature { get; set; }
    [Id(1)]
    public GenericMeasurement<double>? Humidity { get; set; }
    [Id(2)]
    public GenericMeasurement<double>? AmbientLight { get; set; }
    [Id(3)]
    public GenericMeasurement<double>? Pressure { get; set; }
    [Id(4)]
    public GenericMeasurement<double>? VolatileOrganicCompounds { get; set; }
    [Id(5)]
    public GenericMeasurement<double>? Bvoc { get; set; }
    [Id(6)]
    public GenericMeasurement<double>? Co2e { get; set; }
    [Id(7)]
    public GenericMeasurement<double>? SoundMin { get; internal set; }
    [Id(8)]
    public GenericMeasurement<double>? SoundAvg { get; internal set; }
    [Id(9)]
    public GenericMeasurement<double>? SoundMax { get; internal set; }
    [Id(10)]
    public GenericMeasurement<double>? BattVolt { get; set; }

    public static SyneticaMeasurementMetric? operator /(SyneticaMeasurementMetric? lhs, double rhs)
    {
        if (lhs is null) return null;

        return new SyneticaMeasurementMetric
        {
            Temperature = lhs is null ? default : lhs.Temperature.Div(rhs),
            Humidity = lhs is null ? default : lhs.Humidity.Div(rhs),
            AmbientLight = lhs is null ? default : lhs.AmbientLight.Div(rhs),
            Pressure = lhs is null ? default : lhs.Pressure.Div(rhs),
            VolatileOrganicCompounds = lhs is null ? default : lhs.VolatileOrganicCompounds.Div(rhs),
            Bvoc = lhs is null ? default : lhs.Bvoc.Div(rhs),
            Co2e = lhs is null ? default : lhs.Co2e.Div(rhs),
            SoundMin = lhs is null ? default : lhs.SoundMin.Div(rhs),
            SoundAvg = lhs is null ? default : lhs.SoundAvg.Div(rhs),
            SoundMax = lhs is null ? default : lhs.SoundMax.Div(rhs),
            BattVolt = lhs is null ? default : lhs.BattVolt.Div(rhs)
        };
    }

    public static SyneticaMeasurementMetric? operator *(SyneticaMeasurementMetric? lhs, SyneticaMeasurementMetric? rhs)
    {
        if (lhs is null) return rhs;
        if (rhs is null) return lhs;

        return new SyneticaMeasurementMetric
        {
            Temperature = lhs is null ? default : lhs.Temperature.Mul(rhs.Temperature),
            Humidity = lhs is null ? default : lhs.Humidity.Mul(rhs.Humidity),
            AmbientLight = lhs is null ? default : lhs.AmbientLight.Mul(rhs.AmbientLight),
            Pressure = lhs is null ? default : lhs.Pressure.Mul(rhs.Pressure),
            VolatileOrganicCompounds = lhs is null ? default : lhs.VolatileOrganicCompounds.Mul(rhs.VolatileOrganicCompounds),
            Bvoc = lhs is null ? default : lhs.Bvoc.Mul(rhs.Bvoc),
            Co2e = lhs is null ? default : lhs.Co2e.Mul(rhs.Co2e),
            SoundMin = lhs is null ? default : lhs.SoundMin.Mul(rhs.SoundMin),
            SoundAvg = lhs is null ? default : lhs.SoundAvg.Mul(rhs.SoundAvg),
            SoundMax = lhs is null ? default : lhs.SoundMax.Mul(rhs.SoundMax),
            BattVolt = lhs is null ? default : lhs.BattVolt.Mul(rhs.BattVolt)
        };
    }

    public static SyneticaMeasurementMetric? operator +(SyneticaMeasurementMetric? lhs, SyneticaMeasurementMetric? rhs)
    {
        if(lhs is null) return rhs;
        if(rhs is null) return lhs;

        return new SyneticaMeasurementMetric
        {
            Temperature = lhs is null ? default : lhs.Temperature.Add(rhs.Temperature),
            Humidity = lhs is null ? default : lhs.Humidity.Add(rhs.Humidity),
            AmbientLight = lhs is null ? default : lhs.AmbientLight.Add(rhs.AmbientLight),
            Pressure = lhs is null ? default : lhs.Pressure.Add(rhs.Pressure),
            VolatileOrganicCompounds = lhs is null ? default : lhs.VolatileOrganicCompounds.Add(rhs.VolatileOrganicCompounds),
            Bvoc = lhs is null ? default : lhs.Bvoc.Add(rhs.Bvoc),
            Co2e = lhs is null ? default : lhs.Co2e.Add(rhs.Co2e),
            SoundMin = lhs is null ? default : lhs.SoundMin.Add(rhs.SoundMin),
            SoundAvg = lhs is null ? default : lhs.SoundAvg.Add(rhs.SoundAvg),
            SoundMax = lhs is null ? default : lhs.SoundMax.Add(rhs.SoundMax),
            BattVolt = lhs is null ? default : lhs.BattVolt.Add(rhs.BattVolt)
        };
    }

    public static SyneticaMeasurementMetric? operator -(SyneticaMeasurementMetric? lhs, SyneticaMeasurementMetric? rhs)
    {
        if(lhs is null) return rhs;
        if(rhs is null) return lhs;

        return new SyneticaMeasurementMetric
        {
            Temperature = lhs is null ? default : lhs.Temperature.Sub(rhs.Temperature),
            Humidity = lhs is null ? default : lhs.Humidity.Sub(rhs.Humidity),
            AmbientLight = lhs is null ? default : lhs.AmbientLight.Sub(rhs.AmbientLight),
            Pressure = lhs is null ? default : lhs.Pressure.Sub(rhs.Pressure),
            VolatileOrganicCompounds = lhs is null ? default : lhs.VolatileOrganicCompounds.Sub(rhs.VolatileOrganicCompounds),
            Bvoc = lhs is null ? default : lhs.Bvoc.Sub(rhs.Bvoc),
            Co2e = lhs is null ? default : lhs.Co2e.Sub(rhs.Co2e),
            SoundMin = lhs is null ? default : lhs.SoundMin.Sub(rhs.SoundMin),
            SoundAvg = lhs is null ? default : lhs.SoundAvg.Sub(rhs.SoundAvg),
            SoundMax = lhs is null ? default : lhs.SoundMax.Sub(rhs.SoundMax),
            BattVolt = lhs is null ? default : lhs.BattVolt.Sub(rhs.BattVolt)
        };
    }

}
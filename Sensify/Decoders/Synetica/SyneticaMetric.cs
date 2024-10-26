using Sensify.Decoders.Common;

namespace Sensify.Decoders.Synetica;

[GenerateSerializer]
[Alias("Sensify.Decoders.Synetica.SyneticaMetric")]
public partial record SyneticaMetric
{
    [Id(0)]
    public NumericalMetric? Temperature { get; set; }
    [Id(1)]
    public NumericalMetric? Humidity { get; set; }
    [Id(2)]
    public NumericalMetric? AmbientLight { get; set; }
    [Id(3)]
    public NumericalMetric? Pressure { get; set; }
    [Id(4)]
    public NumericalMetric? VolatileOrganicCompounds { get; set; }
    [Id(5)]
    public NumericalMetric? Bvoc { get; set; }
    [Id(6)]
    public NumericalMetric? Co2e { get; set; }
    [Id(7)]
    public NumericalMetric? SoundMin { get; internal set; }
    [Id(8)]
    public NumericalMetric? SoundAvg { get; internal set; }
    [Id(9)]
    public NumericalMetric? SoundMax { get; internal set; }
    [Id(10)]
    public NumericalMetric? BattVolt { get; set; }


    public static SyneticaMetric FromMeasurementMetrics(
        SyneticaMeasurementMetric? min,
        SyneticaMeasurementMetric? max,
        SyneticaMeasurementMetric? average,
        SyneticaMeasurementMetric? range,
        SyneticaMeasurementMetric? std)
    {
        return new()
        {
            Temperature = new(
                min?.Temperature?.Unit ?? MeasurementUnit.None,
                min?.Temperature?.Value,
                max?.Temperature?.Value,
                average?.Temperature?.Value,
                range?.Temperature?.Value,
                std?.Temperature?.Value),

            Humidity = new(min?.Humidity?.Unit ?? MeasurementUnit.None,
                min?.Humidity?.Value,
                max?.Humidity?.Value,
                average?.Humidity?.Value,
                range?.Humidity?.Value,
                std?.Humidity?.Value),


            AmbientLight = new(min?.AmbientLight?.Unit ?? MeasurementUnit.None,
                min?.AmbientLight?.Value,
                max?.AmbientLight?.Value,
                average?.AmbientLight?.Value,
                range?.AmbientLight?.Value,
                std?.AmbientLight?.Value),

            Pressure = new(min?.Pressure?.Unit ?? MeasurementUnit.None,
                min?.Pressure?.Value,
                max?.Pressure?.Value,
                average?.Pressure?.Value,
                range?.Pressure?.Value,
                std?.Pressure?.Value),

            VolatileOrganicCompounds = new(min?.VolatileOrganicCompounds?.Unit ?? MeasurementUnit.None,
                min?.VolatileOrganicCompounds?.Value,
                max?.VolatileOrganicCompounds?.Value,
                average?.VolatileOrganicCompounds?.Value,
                range?.VolatileOrganicCompounds?.Value,
                std?.VolatileOrganicCompounds?.Value),

            Bvoc = new(min?.Bvoc?.Unit ?? MeasurementUnit.None,
                min?.Bvoc?.Value,
                max?.Bvoc?.Value,
                average?.Bvoc?.Value,
                range?.Bvoc?.Value,
                std?.Bvoc?.Value),

            Co2e = new(min?.Co2e?.Unit ?? MeasurementUnit.None,
                min?.Co2e?.Value,
                max?.Co2e?.Value,
                average?.Co2e?.Value,
                range?.Co2e?.Value,
                std?.Co2e?.Value),

            SoundMin = new(min?.SoundMin?.Unit ?? MeasurementUnit.None,
                min?.SoundMin?.Value,
                max?.SoundMin?.Value,
                average?.SoundMin?.Value,
                range?.SoundMin?.Value,
                std?.SoundMin?.Value),

            SoundAvg = new(min?.SoundAvg?.Unit ?? MeasurementUnit.None,
                min?.SoundAvg?.Value,
                max?.SoundAvg?.Value,
                average?.SoundAvg?.Value,
                range?.SoundAvg?.Value,
                std?.SoundAvg?.Value),

            SoundMax = new(min?.SoundMax?.Unit ?? MeasurementUnit.None,
                min?.SoundMax?.Value,
                max?.SoundMax?.Value,
                average?.SoundMax?.Value,
                range?.SoundMax?.Value,
                std?.SoundMax?.Value),

            BattVolt = new(min?.BattVolt?.Unit ?? MeasurementUnit.None,
                min?.BattVolt?.Value,
                max?.BattVolt?.Value,
                average?.BattVolt?.Value,
                range?.BattVolt?.Value,
                std?.BattVolt?.Value),

        };
    }
}

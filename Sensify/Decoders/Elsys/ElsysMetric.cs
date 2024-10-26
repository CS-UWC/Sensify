using Sensify.Decoders.Common;

namespace Sensify.Decoders.Elsys;

[GenerateSerializer]
[Alias("Sensify.Decoders.Elsys.ElsysMetric")]
public partial record ElsysMetric : IMetric
{
    [Id(0)]
    public NumericalMetric? Temperature { get; set; }
    [Id(1)]
    public NumericalMetric? Humidity { get; set; }
    [Id(2)]
    public NumericalMetric? Light { get; set; }
    [Id(3)]
    public NumericalMetric? Motion { get; set; }
    [Id(4)]
    public NumericalMetric? Co2 { get; set; }
    [Id(5)]
    public NumericalMetric? Vdd { get; set; }
    [Id(6)]
    public NumericalMetric? Pulse1Absolute { get; set; }
    [Id(7)]
    public NumericalMetric? Digital { get; set; }
    [Id(8)]
    public NumericalMetric? AccelerationMotion { get; set; }


    public static ElsysMetric FromMeasurementMetrics(
        ElsysMeasurementMetric? min,
        ElsysMeasurementMetric? max,
        ElsysMeasurementMetric? average,
        ElsysMeasurementMetric? range,
        ElsysMeasurementMetric? std)
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

            Light = new(min?.Light?.Unit ?? MeasurementUnit.None,
                min?.Light?.Value,
                max?.Light?.Value,
                average?.Light?.Value,
                range?.Light?.Value,
                std?.Light?.Value),

            Motion = new(min?.Motion?.Unit ?? MeasurementUnit.None,
                min?.Motion?.Value,
                max?.Motion?.Value,
                average?.Motion?.Value,
                range?.Motion?.Value,
                std?.Motion?.Value),

            Co2 = new(min?.Co2?.Unit ?? MeasurementUnit.None,
                min?.Co2?.Value,
                max?.Co2?.Value,
                average?.Co2?.Value,
                range?.Co2?.Value,
                std?.Co2?.Value),

            Vdd = new(min?.Vdd?.Unit ?? MeasurementUnit.None,
                min?.Vdd?.Value,
                max?.Vdd?.Value,
                average?.Vdd?.Value,
                range?.Vdd?.Value,
                std?.Vdd?.Value),

            Pulse1Absolute = new(min?.Pulse1Absolute?.Unit ?? MeasurementUnit.None,
                min?.Pulse1Absolute?.Value,
                max?.Pulse1Absolute?.Value,
                average?.Pulse1Absolute?.Value,
                range?.Pulse1Absolute?.Value,
                std?.Pulse1Absolute?.Value),

            Digital = new(min?.Digital?.Unit ?? MeasurementUnit.None,
                min?.Digital?.Value,
                max?.Digital?.Value,
                average?.Digital?.Value,
                range?.Digital?.Value,
                std?.Digital?.Value),

            AccelerationMotion = new(min?.AccelerationMotion?.Unit ?? MeasurementUnit.None,
                min?.AccelerationMotion?.Value,
                max?.AccelerationMotion?.Value,
                average?.AccelerationMotion?.Value,
                range?.AccelerationMotion?.Value,
                std?.AccelerationMotion?.Value),


        };
    }
}
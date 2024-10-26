using Sensify.Decoders.Common;

namespace Sensify.Decoders.Netvox;

[GenerateSerializer]
[Alias("Sensify.Decoders.Netvox.NetvoxMetric")]
public partial record NetvoxMetric : IMetric
{
    [Id(0)]
    public NumericalMetric? Battery { get; set; }
    [Id(1)]
    public NumericalMetric? Temperature { get; set; }
    [Id(2)]
    public NumericalMetric? Temperature1 { get; set; }
    [Id(3)]
    public NumericalMetric? Temperature2 { get; set; }

    [Id(4)]
    public NumericalMetric? Temperature3 { get; set; }


    public static NetvoxMetric FromMeasurementMetrics(
        NetvoxMeasurementMetric? min,
        NetvoxMeasurementMetric? max,
        NetvoxMeasurementMetric? average,
        NetvoxMeasurementMetric? range,
        NetvoxMeasurementMetric? std)
    {
        return new()
        {

            Battery = new(min?.Battery?.Unit ?? MeasurementUnit.None,
                min?.Battery?.Value,
                max?.Battery?.Value,
                average?.Battery?.Value,
                range?.Battery?.Value,
                std?.Battery?.Value),

            Temperature = new(
                min?.Temperature?.Unit ?? MeasurementUnit.None,
                min?.Temperature?.Value,
                max?.Temperature?.Value,
                average?.Temperature?.Value,
                range?.Temperature?.Value,
                std?.Temperature?.Value),

            Temperature1 = new(min?.Temperature1?.Unit ?? MeasurementUnit.None,
                min?.Temperature1?.Value,
                max?.Temperature1?.Value,
                average?.Temperature1?.Value,
                range?.Temperature1?.Value,
                std?.Temperature1?.Value),

            Temperature2 = new(min?.Temperature2?.Unit ?? MeasurementUnit.None,
                min?.Temperature2?.Value,
                max?.Temperature2?.Value,
                average?.Temperature2?.Value,
                range?.Temperature2?.Value,
                std?.Temperature2?.Value),

            Temperature3 = new(min?.Temperature3?.Unit ?? MeasurementUnit.None,
                min?.Temperature3?.Value,
                max?.Temperature3?.Value,
                average?.Temperature3?.Value,
                range?.Temperature3?.Value,
                std?.Temperature3?.Value),

        };
    }
}
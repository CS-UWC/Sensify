namespace Sensify.Decoders.Common;

[GenerateSerializer]
[Alias("Sensify.Decoders.Common.NumericalMetric")]
public record NumericalMetric(MeasurementUnit Unit, double? Min = default, double?  Max = default, double? Average = default, double? Range = default, double? Std = default)
{
}

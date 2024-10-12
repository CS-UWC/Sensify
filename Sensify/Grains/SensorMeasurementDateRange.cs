namespace Sensify.Grains;

[GenerateSerializer]
[Alias("Sensify.Grains.SensorMeasurementDateRange")]
public record struct SensorMeasurementDateRange(DateTime Start, DateTime End)
{
    public static readonly SensorMeasurementDateRange All = default;

    public static SensorMeasurementDateRange Before(DateTime end) => new(DateTime.MinValue, end.Add(TimeSpan.FromTicks(1)));
    public static SensorMeasurementDateRange After(DateTime start) => new(start.Subtract(TimeSpan.FromTicks(1)), DateTime.MaxValue);

}

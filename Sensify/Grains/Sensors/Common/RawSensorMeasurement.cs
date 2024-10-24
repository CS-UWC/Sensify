namespace Sensify.Grains.Sensors.Common;

[GenerateSerializer]
[Alias("Sensify.Grains.Sensors.Common.RawSensorMeasurement")]
public record struct RawSensorMeasurement
{
    public RawSensorMeasurement() { }
    public RawSensorMeasurement(string hexPayload) : this(hexPayload, null) { }
    public RawSensorMeasurement(string hexPayload, DateTime? timestamp) => (HexPayload, Timestamp) = (hexPayload, timestamp);

    [Id(0)]
    public string HexPayload { get; set; } = null!;
    [Id(1)]
    public DateTime? Timestamp { get; set; }
}

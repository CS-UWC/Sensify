namespace Sensify.Grains.Sensors.Common;

[GenerateSerializer]
[Alias("Sensify.Grains.Sensors.Common.UpdateSensorInfo")]
public record struct UpdateSensorInfo
{
    [Id(0)]
    public SupportedSensorType? SensorType { get; set; }
    [Id(1)]
    public string SensorName { get; set; }
    [Id(2)]
    public string? PayloadDecoder { get; set; }
}

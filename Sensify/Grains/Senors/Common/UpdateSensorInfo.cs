namespace Sensify.Grains.Senors.Common;

[GenerateSerializer]
[Alias("Sensify.Grains.UpdateSensorInfo")]
public record struct UpdateSensorInfo
{
    [Id(0)]
    public SupportedSensorType? SensorType { get; set; }
    [Id(1)]
    public string SensorName { get; set; }
    [Id(2)]
    public string? PayloadDecoder { get; set; }
}

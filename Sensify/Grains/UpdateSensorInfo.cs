namespace Sensify.Grains;

[GenerateSerializer]
[Alias("Sensify.Grains.UpdateSensorInfo")]
public record struct UpdateSensorInfo
{
    [Id(0)]
    public string Id { get; set; }
    [Id(1)]
    public string SensorType { get; set; }
    [Id(2)]
    public string SensorName { get; set; }
    [Id(3)]
    public string PayloadDecoder { get; set; }
}

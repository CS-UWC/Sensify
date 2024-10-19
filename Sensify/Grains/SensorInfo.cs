namespace Sensify.Grains;

[GenerateSerializer]
[Alias("Sensify.Grains.SensorInfo")]
public sealed record SensorInfo
{
    [Id(0)]
    public string Id { get; set; } = string.Empty;
    [Id(1)]
    public SupportedSensorType SensorType { get; set; } = SupportedSensorType.None;
    [Id(2)]
    public string SensorName { get; set; } = string.Empty;
    [Id(3)]
    public string PayloadDecoder { get; set; } = string.Empty;


    public static readonly SensorInfo Empty = new();
}
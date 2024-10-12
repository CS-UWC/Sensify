namespace Sensify.Grains;

[GenerateSerializer]
[Alias("Sensify.Grains.SensorInfo")]
public sealed record class SensorInfo
{
    [Id(0)]
    public string Id { get; set; } = string.Empty;
    [Id(1)]
    public string SensorType { get; set; } = string.Empty;
    [Id(2)]
    public string SensorName { get; set; } = string.Empty;
    [Id(3)]
    public string PayloadDecoder { get; set; } = string.Empty;
}

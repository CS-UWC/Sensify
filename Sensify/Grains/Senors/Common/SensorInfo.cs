using MongoDB.Bson.Serialization.Attributes;
using Sensify.Decoders.Common;

namespace Sensify.Grains.Senors.Common;

[GenerateSerializer]
[Alias("Sensify.Grains.SensorInfo")]
public sealed record SensorInfo
{
    [Id(0)]
    public SensorId? Id { get; set; } = default;
    [Id(1)]
    [BsonSerializer(typeof(MongoSupportedSensorTypeSerializer))]
    public SupportedSensorType SensorType { get; set; } = SupportedSensorType.Generic;
    [Id(2)]
    public string SensorName { get; set; } = string.Empty;
    [Id(3)]
    public string PayloadDecoder { get; set; } = string.Empty;


    public static readonly SensorInfo Empty = new();
}
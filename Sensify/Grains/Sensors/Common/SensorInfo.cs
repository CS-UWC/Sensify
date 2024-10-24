using MongoDB.Bson.Serialization.Attributes;
using Sensify.Decoders.Common;
using System.Text.Json.Serialization;

namespace Sensify.Grains.Sensors.Common;

[GenerateSerializer]
[Alias("Sensify.Grains.Sensors.Common.SensorInfo")]
public sealed record SensorInfo
{
    [Id(0)]
    [JsonConverter(typeof(JsonConverterNullableSensorId))]
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
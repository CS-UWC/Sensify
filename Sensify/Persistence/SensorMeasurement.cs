using MongoDB.Bson.Serialization.Attributes;

namespace Sensify.Persistence;

[BsonIgnoreExtraElements]
public class SensorMeasurement<TMeasurement>
{
    [BsonElement("sensorId")]
    public string SensorId { get; set; } = null!;
    [BsonElement("timestamp")]
    public DateTime Timestamp { get; set; }

    [BsonElement("measurement")]
    public required TMeasurement Measurement { get; set; }
}

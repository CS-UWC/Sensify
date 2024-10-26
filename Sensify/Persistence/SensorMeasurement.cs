using MongoDB.Bson.Serialization.Attributes;

namespace Sensify.Persistence;

[BsonIgnoreExtraElements]
[GenerateSerializer]
[Alias("Sensify.Persistence.SensorMeasurement`1")]
public class SensorMeasurement<TMeasurement> : ISensorMeasurement
{
    [BsonElement("sensorId")]
    [Id(0)]
    public string SensorId { get; set; } = null!;
    [BsonElement("timestamp")]
    [Id(1)]
    public DateTime Timestamp { get; set; }

    [BsonElement("measurement")]
    [BsonIgnoreIfDefault]
    [Id(2)]
    public required TMeasurement Measurement { get; set; }

    object? ISensorMeasurement.Measurement => Measurement;
}

public interface ISensorMeasurement
{
    public string SensorId { get; }
    public DateTime Timestamp { get; }
    public object? Measurement { get; }
}

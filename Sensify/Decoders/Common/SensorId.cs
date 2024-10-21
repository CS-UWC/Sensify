using MongoDB.Bson.Serialization.Attributes;
using Sensify.Grains.Senors.Common;

namespace Sensify.Decoders.Common;

[GenerateSerializer]
[Alias("Sensify.Decoders.Common.SensorId")]
public readonly record struct SensorId
{
    [Id(0)]
    public string Id { get; }
    [Id(1)]
    [BsonSerializer(typeof(MongoSupportedSensorTypeSerializer))]
    public SupportedSensorType SensorType { get; }

    public SensorId(string id, SupportedSensorType sensorType)
    {
        Id = id;
        SensorType = sensorType;
    }

    public override string ToString()
    {
        return $"{(int)SensorType}:{Id}";
    }

    public static SensorId From(ReadOnlySpan<char> sensorId)
    {
        var sensorTypeIndex = sensorId.IndexOf(':');
        var type = sensorId[..sensorTypeIndex];
        var id = sensorId[(sensorTypeIndex + 1)..].Trim();

        return new(id.ToString(), (SupportedSensorType)int.Parse(type.Trim()));
    }

    public static bool IsValid(ReadOnlySpan<char> sensorId)
    {
        var sensorTypeIndex = sensorId.Trim().IndexOf(':');

        if(sensorTypeIndex < 0 || sensorTypeIndex == sensorId.Length) return false;

        var strType = sensorId[..sensorTypeIndex].Trim();
        var strId = sensorId[(sensorTypeIndex + 1)..].Trim();

        return strId.Length > 0 && strType.Length > 0 && int.TryParse(strType, out _);


    }
}

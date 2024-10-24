using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using Sensify.Extensions;
using Sensify.Grains.Sensors.Common;

namespace Sensify.Decoders.Common;

internal class MongoSupportedSensorTypeSerializer : EnumSerializer<SupportedSensorType>
{

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, SupportedSensorType value)
    {
        var writer = context.Writer;

        writer.WriteString(value.AsString());
    }

    public override SupportedSensorType Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        IBsonReader reader = context.Reader;
        BsonType currentBsonType = reader.GetCurrentBsonType();
        return currentBsonType switch
        {
            BsonType.Int32 => (SupportedSensorType)reader.ReadInt32(),
            BsonType.Int64 => (SupportedSensorType)reader.ReadInt64(),
            BsonType.Double => (SupportedSensorType)(long)reader.ReadDouble(),
            BsonType.String => reader.ReadString().AsSupportedSensorType(),
            _ => throw CreateCannotDeserializeFromBsonTypeException(currentBsonType),
        };
    }
}
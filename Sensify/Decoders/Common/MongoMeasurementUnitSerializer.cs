using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using Sensify.Extensions;

namespace Sensify.Decoders.Common;

internal class MongoMeasurementUnitSerializer : EnumSerializer<MeasurementUnit>
{

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, MeasurementUnit value)
    {
        var writer = context.Writer;

        writer.WriteString(value.AsString());
    }

    public override MeasurementUnit Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        IBsonReader reader = context.Reader;
        BsonType currentBsonType = reader.GetCurrentBsonType();
        return currentBsonType switch
        {
            BsonType.Int32 => (MeasurementUnit)reader.ReadInt32(),
            BsonType.Int64 => (MeasurementUnit)reader.ReadInt64(),
            BsonType.Double => (MeasurementUnit)(long)reader.ReadDouble(),
            BsonType.String => reader.ReadString().AsMeasurementUnit(),
            _ => throw CreateCannotDeserializeFromBsonTypeException(currentBsonType),
        };
    }
}
using MongoDB.Bson.Serialization.Attributes;

namespace Sensify.Decoders.Common;

[GenerateSerializer]
[Alias("Sensify.Decoders.Common.GenericMeasurement`1")]
[BsonIgnoreExtraElements]
public record GenericMeasurement<TValue>
{
    [Id(0)]
    public TValue Value { get; init; }
    [Id(1)]
    [BsonSerializer(typeof(MongoMeasurementUnitSerializer))]
    public MeasurementUnit Unit { get; init; }
    public GenericMeasurement(TValue value, MeasurementUnit unit)
    {
        Value = value;
        Unit = unit;
    }

    public GenericMeasurement(TValue value) : this(value, MeasurementUnit.None) { }
}

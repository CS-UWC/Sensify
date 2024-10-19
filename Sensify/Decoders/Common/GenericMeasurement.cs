using MongoDB.Bson.Serialization.Attributes;

namespace Sensify.Decoders.Common;

[GenerateSerializer]
[Alias("Sensify.Decoders.Common.GenericMeasurement`1")]
public record GenericMeasurement<TValue>(TValue Value, MeasurementUnit Unit)
{
    public GenericMeasurement(TValue value) : this(value, MeasurementUnit.None) { }
}


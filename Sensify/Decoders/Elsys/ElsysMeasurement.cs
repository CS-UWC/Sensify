using MongoDB.Bson.Serialization.Attributes;
using Sensify.Decoders.Common;

namespace Sensify.Decoders.Elsys;

[GenerateSerializer]
[Alias("Sensify.Decoders.Elsys.ElsysMeasurement")]
[BsonIgnoreExtraElements]
public partial record ElsysMeasurement
{
    [Id(0)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<float>? Temperature { get; set; }
    [Id(1)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<byte>? Humidity { get; set; }
    [Id(2)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<Vector3<sbyte>>? Acceleration { get; set; }
    [Id(3)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<ushort>? Light { get; set; }
    [Id(4)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<byte>? Motion { get; set; }
    [Id(5)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<ushort>? Co2 { get; set; }
    [Id(6)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<ushort>? Vdd { get; set; }
    [BsonIgnoreIfDefault]
    [Id(7)]
    public GenericMeasurement<uint>? Pulse1Absolute { get; set; }
    [BsonIgnoreIfDefault]
    [Id(8)]
    public GenericMeasurement<byte>? Digital { get; set; }
    [BsonIgnoreIfDefault]
    [Id(9)]
    public GenericMeasurement<byte>? AccelerationMotion { get; set; }

}
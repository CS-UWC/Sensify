using MongoDB.Bson.Serialization.Attributes;
using Sensify.Decoders.Common;
using System.Numerics;

namespace Sensify.Decoders.Netvox;

[GenerateSerializer]
[Alias("Sensify.Decoders.Netvox.NetvoxMeasurement")]
[BsonIgnoreExtraElements]
[BsonNoId]
public partial record NetvoxMeasurement : IAdditionOperators<NetvoxMeasurement, NetvoxMeasurement, NetvoxMeasurement>
{
    [BsonIgnoreIfDefault]
    [Id(0)]
    public GenericMeasurement<float>? Battery { get; set; }
    [BsonIgnoreIfDefault]
    [Id(1)]
    public GenericMeasurement<Vector3<float>>? Acceleration { get; set; }
    [BsonIgnoreIfDefault]
    [Id(2)]
    public GenericMeasurement<Vector3<float>>? Velocity { get; set; }
    [BsonIgnoreIfDefault]
    [Id(3)]
    public GenericMeasurement<float>? Temperature { get; set; }
    [Id(4)]
    public GenericMeasurement<float>? Temperature1 { get; set; }
    [Id(5)]
    public GenericMeasurement<float>? Temperature2 { get; set; }
    [Id(6)]
    public GenericMeasurement<float>? Temperature3 { get; set; }
    [Id(7)]
    public GenericMeasurement<Switch>? Contact { get; set; }

}

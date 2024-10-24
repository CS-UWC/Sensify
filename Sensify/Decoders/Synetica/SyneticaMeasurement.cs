using MongoDB.Bson.Serialization.Attributes;
using Sensify.Decoders.Common;
using System.Numerics;

namespace Sensify.Decoders.Synetica;

[GenerateSerializer]
[BsonIgnoreExtraElements]
[BsonNoId]
[Alias("Sensify.Decoders.Synetica.SyneticaMeasurement")]
public partial record SyneticaMeasurement : IAdditionOperators<SyneticaMeasurement, SyneticaMeasurement, SyneticaMeasurement>
{
    [Id(0)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<float>? Temperature { get; set; }
    [Id(1)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<byte>? Humidity { get; set; }
    [Id(2)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<ushort>? AmbientLight { get; set; }
    [Id(3)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<ushort>? Pressure { get; set; }
    [Id(4)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<ushort>? VolatileOrganicCompounds { get; set; }
    [BsonIgnoreIfDefault]
    [Id(5)]
    public GenericMeasurement<float>? Bvoc { get; set; }
    [BsonIgnoreIfDefault]
    [Id(6)]
    public GenericMeasurement<float>? Co2e { get; set; }
    [BsonIgnoreIfDefault]
    [Id(7)]
    public GenericMeasurement<float>? SoundMin { get; internal set; }
    [BsonIgnoreIfDefault]
    [Id(8)]
    public GenericMeasurement<float>? SoundAvg { get; internal set; }
    [BsonIgnoreIfDefault]
    [Id(9)]
    public GenericMeasurement<float>? SoundMax { get; internal set; }
    [BsonIgnoreIfDefault]
    [Id(10)]
    public GenericMeasurement<float>? BattVolt { get; set; }
    
}

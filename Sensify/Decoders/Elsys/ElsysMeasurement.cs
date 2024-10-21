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
    public GenericMeasurement<ushort>? Ligth { get; set; }
    [Id(4)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<byte>? Motion { get; set; }
    [Id(5)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<ushort>? Co2 { get; set; }
    [Id(6)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<ushort>? Vdd { get; set; }
    [Id(7)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<ushort>? Analog1 { get; set; }
    [Id(8)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<Location<float>>? Gps { get; set; }
    [Id(9)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<ushort>? Pulse1 { get; set; }
    [Id(10)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<uint>? Pulse1Absolute { get; set; }
    [Id(11)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<float>? ExternalTemperature { get; set; }
    [Id(12)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<byte>? Digital { get; set; }
    [Id(13)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<ushort>? Distance { get; set; }
    [Id(14)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<byte>? AccelerationMotion { get; set; }
    [Id(15)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<float>? InfraRedInternalTemperature { get; set; }
    [Id(16)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<float>? InfraRedExternalTemperature { get; set; }
    [Id(17)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<byte>? Occupancy { get; set; }
    [Id(18)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<byte>? WaterLeak { get; set; }
    [Id(19)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<float[]>? Grideye { get; set; }
    [Id(20)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<float>? Pressure { get; set; }
    [Id(21)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<float>? SoundPeak { get; set; }
    [Id(22)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<float>? SoundAverage { get; set; }
    [Id(23)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<ushort>? Pulse2 { get; set; }
    [Id(24)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<uint>? PulseAbsolute2 { get; set; }
    [Id(25)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<ushort>? Analog2 { get; set; }
    [Id(26)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<List<float>>? ExternalTemperature2 { get; set; }
    [Id(27)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<byte>? Digital2 { get; set; }
    [Id(28)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<int>? AnalogUv { get; set; }
    [Id(29)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<int>? Tvoc { get; set; }

}
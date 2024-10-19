using Sensify.Decoders.Common;

namespace Sensify.Decoders.Elsys;

[GenerateSerializer]
[Alias("Sensify.Decoders.Elsys.ElsysMeasurement")]
public partial record ElsysMeasurement
{
    [Id(0)]
    public GenericMeasurement<float>? Temperature { get; set; }
    [Id(1)]
    public GenericMeasurement<byte>? Humidity { get; set; }
    [Id(2)]
    public GenericMeasurement<Vector3<sbyte>>? Acceleration { get; set; }
    [Id(3)]
    public GenericMeasurement<ushort>? Ligth { get; set; }
    [Id(4)]
    public GenericMeasurement<byte>? Motion { get; set; }
    [Id(5)]
    public GenericMeasurement<ushort>? Co2 { get; set; }
    [Id(6)]
    public GenericMeasurement<ushort>? Vdd { get; set; }
    [Id(7)]
    public GenericMeasurement<ushort>? Analog1 { get; set; }
    [Id(8)]
    public GenericMeasurement<Location<float>>? Gps { get; set; }
    [Id(9)]
    public GenericMeasurement<ushort>? Pulse1 { get; set; }
    [Id(10)]
    public GenericMeasurement<uint>? Pulse1Absolute { get; set; }
    [Id(11)]
    public GenericMeasurement<float>? ExternalTemperature { get; set; }
    [Id(12)]
    public GenericMeasurement<byte>? Digital { get; set; }
    [Id(13)]
    public GenericMeasurement<ushort>? Distance { get; set; }
    [Id(14)]
    public GenericMeasurement<byte>? AccelerationMotion { get; set; }
    [Id(15)]
    public GenericMeasurement<float>? InfraRedInternalTemperature { get; set; }
    [Id(16)]
    public GenericMeasurement<float>? InfraRedExternalTemperature { get; set; }
    [Id(17)]
    public GenericMeasurement<byte>? Occupancy { get; set; }
    [Id(18)]
    public GenericMeasurement<byte>? WaterLeak { get; set; }
    [Id(19)]
    public GenericMeasurement<float[]>? Grideye { get; set; }
    [Id(20)]
    public GenericMeasurement<float>? Pressure { get; set; }
    [Id(21)]
    public GenericMeasurement<float>? SoundPeak { get; set; }
    [Id(22)]
    public GenericMeasurement<float>? SoundAverage { get; set; }
    [Id(23)]
    public GenericMeasurement<ushort>? Pulse2 { get; set; }
    [Id(24)]
    public GenericMeasurement<uint>? PulseAbsolute2 { get; set; }
    [Id(25)]
    public GenericMeasurement<ushort>? Analog2 { get; set; }
    [Id(26)]
    public GenericMeasurement<List<float>>? ExternalTemperature2 { get; set; }
    [Id(27)]
    public GenericMeasurement<byte>? Digital2 { get; set; }
    [Id(28)]
    public GenericMeasurement<int>? AnalogUv { get; set; }
    [Id(29)]
    public GenericMeasurement<int>? Tvoc { get; set; }

}
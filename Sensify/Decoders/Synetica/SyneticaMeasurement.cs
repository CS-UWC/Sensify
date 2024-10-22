using MongoDB.Bson.Serialization.Attributes;
using Sensify.Decoders.Common;

namespace Sensify.Decoders.Synetica;

[GenerateSerializer]
[BsonIgnoreExtraElements]
[BsonNoId]
[Alias("Sensify.Decoders.Synetica.SyneticaMeasurement")]
public record SyneticaMeasurement
{
    [Id(0)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<float>? Temperature { get; set; }
    [Id(1)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<byte>? Humidity { get; set; }
    [Id(2)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<ushort>? Lux { get; set; }
    [Id(3)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<ushort>? PressureMbar { get; set; }
    [Id(4)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<ushort>? Iaq { get; set; }
    [Id(5)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<float>? O2Percentage { get; set; }
    [Id(6)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<float>? CarbonMonoxide { get; set; }
    [Id(7)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<ushort>? CarbonDioxide { get; set; }
    [Id(8)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<float>? OzonePpm { get; set; }
    [Id(9)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<float>? OzonePpb { get; set; }
    [Id(10)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<float>? PollutantsKohm { get; set; }
    [Id(11)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<ushort>? Pm25 { get; set; }
    [Id(12)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<ushort>? Pm10 { get; set; }
    [Id(13)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<float>? HydrogenSulfide { get; set; }
    [Id(14)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<List<Tuple<byte, uint>>>? Counter { get; set; }
    [Id(15)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<List<Tuple<byte, byte>>>? MbExceptions { get; set; }
    [Id(16)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<List<Tuple<byte, float>>>? MbIntervalValues { get; set; }
    [Id(17)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<List<Tuple<byte, float>>>? MbCumulativeValues { get; set; }
    [Id(18)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<float>? Bvoc { get; set; }
    [Id(19)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<uint>? PirDetectionCount { get; set; }
    [Id(20)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<uint>? PirOccupiedTimeSeconds { get; set; }
    [Id(21)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<float>? TempProbe1 { get; set; }
    [Id(22)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<float>? TempProbe2 { get; set; }
    [Id(23)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<float>? TempProbe3 { get; set; }
    [Id(24)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<uint>? TempProbeInBandDurationS1 { get; set; }
    [Id(25)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<uint>? TempProbeInBandDurationS2 { get; set; }
    [Id(26)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<uint>? TempProbeInBandDurationS3 { get; set; }
    [Id(27)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<ushort>? TempProbeInBandAlarmCount1 { get; set; }
    [Id(28)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<ushort>? TempProbeInBandAlarmCount2 { get; set; }
    [Id(29)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<ushort>? TempProbeInBandAlarmCount3 { get; set; }
    [Id(30)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<uint>? TempProbeLowDurationS1 { get; set; }
    [Id(31)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<uint>? TempProbeLowDurationS2 { get; set; }
    [Id(32)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<uint>? TempProbeLowDurationS3 { get; set; }
    [Id(33)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<ushort>? TempProbeLowAlarmCount1 { get; set; }
    [Id(34)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<ushort>? TempProbeLowAlarmCount2 { get; set; }
    [Id(35)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<ushort>? TempProbeLowAlarmCount3 { get; set; }
    [Id(36)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<uint>? TempProbeHighDurationS1 { get; set; }
    [Id(37)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<uint>? TempProbeHighDurationS2 { get; set; }
    [Id(38)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<uint>? TempProbeHighDurationS3 { get; set; }
    [Id(39)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<ushort>? TempProbeHighAlarmCount1 { get; set; }
    [Id(40)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<ushort>? TempProbeHighAlarmCount2 { get; set; }
    [Id(41)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<ushort>? TempProbeHighAlarmCount3 { get; set; }
    [Id(42)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<short>? DiffPressure { get; set; }
    [Id(43)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<ushort>? Voltage { get; set; }
    [Id(44)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<ushort>? Current { get; set; }
    [Id(45)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<ushort>? Resistance { get; set; }
    [Id(46)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<bool>? LeakDetectEvent { get; set; }
    [Id(47)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<bool>? VibrationEvent { get; set; }
    [Id(48)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<ushort>? RangeMm { get; set; }
    [Id(49)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<uint>? RangeInBandDurationS { get; set; }
    [Id(50)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<ushort>? RangeInBandAlarmCount { get; set; }
    [Id(51)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<uint>? RangeLowDurationS { get; set; }
    [Id(52)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<ushort>? RangeLowAlarmCount { get; set; }
    [Id(53)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<uint>? RangeHighDurationS { get; set; }
    [Id(54)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<ushort>? RangeHighAlarmCount { get; set; }
    [Id(55)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<ushort>? PressureTxMbar { get; set; }
    [Id(56)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<float>? TemperatureTxDegC { get; set; }
    [Id(57)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<float>? RangeAmpl { get; set; }
    [Id(58)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<float>? Co2ePpm { get; set; }
    [Id(59)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<float>? SoundMinDbA { get; internal set; }
    [Id(60)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<float>? SoundAvgDbA { get; internal set; }
    [Id(61)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<float>? SoundMaxDbA { get; internal set; }

    [Id(62)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<double>? CpuTemp { get; set; }
    [Id(63)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<byte>? BattStatus { get; set; }
    [Id(64)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<float>? BattVolt { get; set; }
    [Id(65)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<short>? RxRssi { get; set; }
    [Id(66)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<sbyte>? RxSnr { get; set; }
    [Id(67)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<ushort>? RxCount { get; set; }
    [Id(68)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<ushort>? TxTimeMs { get; set; }
    [Id(69)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<sbyte>? TxPowerDbm { get; set; }
    [Id(70)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<ushort>? TxCount { get; set; }
    [Id(71)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<ushort>? PowerUpCount { get; set; }
    [Id(72)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<ushort>? UsbInCount { get; set; }
    [Id(73)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<ushort>? LoginOkCount { get; set; }
    [Id(74)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<ushort>? LoginFailCount { get; set; }
    [Id(75)]
    [BsonIgnoreIfDefault]
    public GenericMeasurement<uint>? FanRunTimeS { get; set; }
}

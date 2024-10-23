using Sensify.Decoders.Common;
using Sensify.Extensions;
using System.Runtime.CompilerServices;

namespace Sensify.Decoders.Synetica;

public sealed class SyneticaDecoder
{
    public SyneticaMeasurement Decode(string payload) => Decode(payload.ToHexBytes());

    public SyneticaMeasurement Decode(ReadOnlySpan<byte> bytes)
    {
        var result = new SyneticaMeasurement();


        for (int i = 0; i < bytes.Length; i++)
        {
            switch ((SyneticaDataUpType)bytes[i])
            {
                case SyneticaDataUpType.Temp:
                    result.Temperature = new((short)(bytes[i + 1] << 8 | bytes[i + 2]) / 10f, MeasurementUnit.Celsius);
                    i += 2;
                    break;
                case SyneticaDataUpType.Rh:
                    result.Humidity = new(bytes[i + 1], MeasurementUnit.Percentage);
                    i++;
                    break;
                case SyneticaDataUpType.Lux:
                    result.Lux = new((ushort)(bytes[i + 1] << 8 | bytes[i + 2]), MeasurementUnit.Lux);
                    i += 2;
                    break;
                case SyneticaDataUpType.Pressure:
                    result.PressureMbar = new((ushort)(bytes[i + 1] << 8 | bytes[i + 2]), MeasurementUnit.Millibar);
                    i += 2;
                    break;
                case SyneticaDataUpType.VocIaq:
                    result.Iaq = new((ushort)(bytes[i + 1] << 8 | bytes[i + 2]), MeasurementUnit.IaqIndex);
                    i += 2;
                    break;
                case SyneticaDataUpType.O2Perc:
                    result.O2Percentage = new(bytes[i + 1] / 10f, MeasurementUnit.Percentage);
                    i++;
                    break;
                case SyneticaDataUpType.Co:
                    result.CarbonMonoxide = new((bytes[i + 1] << 8 | bytes[i + 2]) / 100f, MeasurementUnit.PartsPerMillion);
                    i += 2;
                    break;
                case SyneticaDataUpType.Co2:
                    result.CarbonDioxide = new((ushort)(bytes[i + 1] << 8 | bytes[i + 2]), MeasurementUnit.PartsPerMillion);
                    i += 2;
                    break;
                case SyneticaDataUpType.Ozone:
                    result.OzonePpm = new((bytes[i + 1] << 8 | bytes[i + 2]) / 10000f, MeasurementUnit.PartsPerMillion);
                    result.OzonePpb = new((bytes[i + 1] << 8 | bytes[i + 2]) / 10f, MeasurementUnit.PartsPerBillion);
                    i += 2;
                    break;
                case SyneticaDataUpType.Pollutants:
                    result.PollutantsKohm = new((bytes[i + 1] << 8 | bytes[i + 2]) / 10f, MeasurementUnit.KiloOhms);
                    i += 2;
                    break;
                case SyneticaDataUpType.Pm25:
                    result.Pm25 = new((ushort)(bytes[i + 1] << 8 | bytes[i + 2]), MeasurementUnit.UgPerM3);
                    i += 2;
                    break;
                case SyneticaDataUpType.Pm10:
                    result.Pm10 = new((ushort)(bytes[i + 1] << 8 | bytes[i + 2]), MeasurementUnit.UgPerM3);
                    i += 2;
                    break;
                case SyneticaDataUpType.H2s:
                    result.HydrogenSulfide = new((bytes[i + 1] << 8 | bytes[i + 2]) / 100f, MeasurementUnit.PartsPerMillion);
                    i += 2;
                    break;
                case SyneticaDataUpType.Counter:
                    var counterValue = (uint)((bytes[i + 2] << 24) | (bytes[i + 3] << 16) | (bytes[i + 4] << 8) | bytes[i + 5]);
                    if (result.Counter == null)
                    {
                        result.Counter = new([]);
                    }
                    result.Counter.Value.Add(new Tuple<byte, uint>(bytes[i + 1], counterValue));
                    i += 5;
                    break;
                case SyneticaDataUpType.MbException:
                    if (result.MbExceptions == null)
                    {
                        result.MbExceptions = new([]);
                    }
                    result.MbExceptions.Value.Add(new Tuple<byte, byte>(bytes[i + 1], bytes[i + 2]));
                    i += 2;
                    break;
                case SyneticaDataUpType.MbInterval:
                    var intervalValue = Unsafe.BitCast<int, float>((bytes[i + 2] << 24) | (bytes[i + 3] << 16) | (bytes[i + 4] << 8) | bytes[i + 5]);
                    if (result.MbIntervalValues == null)
                    {
                        result.MbIntervalValues = new([]);
                    }
                    result.MbIntervalValues.Value.Add(new Tuple<byte, float>(bytes[i + 1], intervalValue));
                    i += 5;
                    break;
                case SyneticaDataUpType.MbCumulative:
                    var cumulativeValue = Unsafe.BitCast<int,float>((bytes[i + 2] << 24) | (bytes[i + 3] << 16) | (bytes[i + 4] << 8) | bytes[i + 5]);
                    if (result.MbCumulativeValues == null)
                    {
                        result.MbCumulativeValues = new([]);
                    }
                    result.MbCumulativeValues.Value.Add(new Tuple<byte, float>(bytes[i + 1], cumulativeValue));
                    i += 5;
                    break;
                case SyneticaDataUpType.Voc:
                    result.Bvoc = new(Unsafe.BitCast<int,float>((bytes[i + 2] << 24) | (bytes[i + 3] << 16) | (bytes[i + 4] << 8) | bytes[i + 5]), MeasurementUnit.PartsPerMillion);
                    i += 4;
                    break;
                case SyneticaDataUpType.PirCount:
                    result.PirDetectionCount = new((uint)((bytes[i + 1] << 24) | (bytes[i + 2] << 16) | (bytes[i + 3] << 8) | bytes[i + 4]));
                    i += 4;
                    break;
                case SyneticaDataUpType.PirOccTime:
                    result.PirOccupiedTimeSeconds = new((uint)((bytes[i + 1] << 24) | (bytes[i + 2] << 16) | (bytes[i + 3] << 8) | bytes[i + 4]));
                    i += 4;
                    break;
                case SyneticaDataUpType.TempProbe1:
                    result.TempProbe1 = new((short)(bytes[i + 1] << 8 | bytes[i + 2]) / 10f, MeasurementUnit.Celsius);
                    i += 2;
                    break;
                case SyneticaDataUpType.TempProbe2:
                    result.TempProbe2 = new((short)(bytes[i + 1] << 8 | bytes[i + 2]) / 10f, MeasurementUnit.Celsius);
                    i += 2;
                    break;
                case SyneticaDataUpType.TempProbe3:
                    result.TempProbe3 = new((short)(bytes[i + 1] << 8 | bytes[i + 2]) / 10f, MeasurementUnit.Celsius);
                    i += 2;
                    break;
                case SyneticaDataUpType.TempProbeInBandDurationS1:
                    result.TempProbeInBandDurationS1 = new((uint)((bytes[i + 1] << 24) | (bytes[i + 2] << 16) | (bytes[i + 3] << 8) | bytes[i + 4]));
                    i += 4;
                    break;
                case SyneticaDataUpType.TempProbeInBandDurationS2:
                    result.TempProbeInBandDurationS2 = new((uint)((bytes[i + 1] << 24) | (bytes[i + 2] << 16) | (bytes[i + 3] << 8) | bytes[i + 4]));
                    i += 4;
                    break;
                case SyneticaDataUpType.TempProbeInBandDurationS3:
                    result.TempProbeInBandDurationS3 = new((uint)((bytes[i + 1] << 24) | (bytes[i + 2] << 16) | (bytes[i + 3] << 8) | bytes[i + 4]));
                    i += 4;
                    break;
                case SyneticaDataUpType.TempProbeInBandAlarmCount1:
                    result.TempProbeInBandAlarmCount1 = new((ushort)((bytes[i + 1] << 8) | bytes[i + 2]));
                    i += 2;
                    break;
                case SyneticaDataUpType.TempProbeInBandAlarmCount2:
                    result.TempProbeInBandAlarmCount2 = new((ushort)((bytes[i + 1] << 8) | bytes[i + 2]));
                    i += 2;
                    break;
                case SyneticaDataUpType.TempProbeInBandAlarmCount3:
                    result.TempProbeInBandAlarmCount3 = new((ushort)((bytes[i + 1] << 8) | bytes[i + 2]));
                    i += 2;
                    break;
                case SyneticaDataUpType.TempProbeLowDurationS1:
                    result.TempProbeLowDurationS1 = new((uint)((bytes[i + 1] << 24) | (bytes[i + 2] << 16) | (bytes[i + 3] << 8) | bytes[i + 4]));
                    i += 4;
                    break;
                case SyneticaDataUpType.TempProbeLowDurationS2:
                    result.TempProbeLowDurationS2 = new((uint)((bytes[i + 1] << 24) | (bytes[i + 2] << 16) | (bytes[i + 3] << 8) | bytes[i + 4]));
                    i += 4;
                    break;
                case SyneticaDataUpType.TempProbeLowDurationS3:
                    result.TempProbeLowDurationS3 = new((uint)((bytes[i + 1] << 24) | (bytes[i + 2] << 16) | (bytes[i + 3] << 8) | bytes[i + 4]));
                    i += 4;
                    break;
                case SyneticaDataUpType.TempProbeLowAlarmCount1:
                    result.TempProbeLowAlarmCount1 = new((ushort)((bytes[i + 1] << 8) | bytes[i + 2]));
                    i += 2;
                    break;
                case SyneticaDataUpType.TempProbeLowAlarmCount2:
                    result.TempProbeLowAlarmCount2 = new((ushort)((bytes[i + 1] << 8) | bytes[i + 2]));
                    i += 2;
                    break;
                case SyneticaDataUpType.TempProbeLowAlarmCount3:
                    result.TempProbeLowAlarmCount3 = new((ushort)((bytes[i + 1] << 8) | bytes[i + 2]));
                    i += 2;
                    break;
                case SyneticaDataUpType.TempProbeHighDurationS1:
                    result.TempProbeHighDurationS1 = new((uint)((bytes[i + 1] << 24) | (bytes[i + 2] << 16) | (bytes[i + 3] << 8) | bytes[i + 4]));
                    i += 4;
                    break;
                case SyneticaDataUpType.TempProbeHighDurationS2:
                    result.TempProbeHighDurationS2 = new((uint)((bytes[i + 1] << 24) | (bytes[i + 2] << 16) | (bytes[i + 3] << 8) | bytes[i + 4]));
                    i += 4;
                    break;
                case SyneticaDataUpType.TempProbeHighDurationS3:
                    result.TempProbeHighDurationS3 = new((uint)((bytes[i + 1] << 24) | (bytes[i + 2] << 16) | (bytes[i + 3] << 8) | bytes[i + 4]));
                    i += 4;
                    break;
                case SyneticaDataUpType.TempProbeHighAlarmCount1:
                    result.TempProbeHighAlarmCount1 = new((ushort)((bytes[i + 1] << 8) | bytes[i + 2]));
                    i += 2;
                    break;
                case SyneticaDataUpType.TempProbeHighAlarmCount2:
                    result.TempProbeHighAlarmCount2 = new((ushort)((bytes[i + 1] << 8) | bytes[i + 2]));
                    i += 2;
                    break;
                case SyneticaDataUpType.TempProbeHighAlarmCount3:
                    result.TempProbeHighAlarmCount3 = new((ushort)((bytes[i + 1] << 8) | bytes[i + 2]));
                    i += 2;
                    break;
                case SyneticaDataUpType.DiffPressure:
                    result.DiffPressure = new((short)((bytes[i + 1] << 8) | bytes[i + 2]), MeasurementUnit.Pascal);
                    i += 2;
                    break;
                case SyneticaDataUpType.Voltage:
                    result.Voltage = new((ushort)((bytes[i + 1] << 8) | bytes[i + 2]), MeasurementUnit.Volts);
                    i += 2;
                    break;
                case SyneticaDataUpType.Current:
                    result.Current = new((ushort)((bytes[i + 1] << 8) | bytes[i + 2]), MeasurementUnit.MilliAmperes);
                    i += 2;
                    break;
                case SyneticaDataUpType.Resistance:
                    result.Resistance = new((ushort)((bytes[i + 1] << 8) | bytes[i + 2]), MeasurementUnit.KiloOhms);
                    i += 2;
                    break;
                case SyneticaDataUpType.LeakDetectEvt:
                    result.LeakDetectEvent = new((bytes[i + 1] != 0));
                    i++;
                    break;
                case SyneticaDataUpType.VibrationEvt:
                    result.VibrationEvent = new(bytes[i + 1] != 0);
                    i++;
                    break;
                case SyneticaDataUpType.RangeMm:
                    result.RangeMm = new((ushort)((bytes[i + 1] << 8) | bytes[i + 2]), MeasurementUnit.Millimeters);
                    i += 2;
                    break;
                case SyneticaDataUpType.RangeInBandDurationS:
                    result.RangeInBandDurationS = new((uint)((bytes[i + 1] << 24) | (bytes[i + 2] << 16) | (bytes[i + 3] << 8) | bytes[i + 4]), MeasurementUnit.Seconds);
                    i += 4;
                    break;
                case SyneticaDataUpType.RangeInBandAlarmCount:
                    result.RangeInBandAlarmCount = new((ushort)((bytes[i + 1] << 8) | bytes[i + 2]));
                    i += 2;
                    break;
                case SyneticaDataUpType.RangeLowDurationS:
                    result.RangeLowDurationS = new((uint)((bytes[i + 1] << 24) | (bytes[i + 2] << 16) | (bytes[i + 3] << 8) | bytes[i + 4]), MeasurementUnit.Seconds);
                    i += 4;
                    break;
                case SyneticaDataUpType.RangeLowAlarmCount:
                    result.RangeLowAlarmCount = new((ushort)((bytes[i + 1] << 8) | bytes[i + 2]));
                    i += 2;
                    break;
                case SyneticaDataUpType.RangeHighDurationS:
                    result.RangeHighDurationS = new((uint)((bytes[i + 1] << 24) | (bytes[i + 2] << 16) | (bytes[i + 3] << 8) | bytes[i + 4]), MeasurementUnit.Seconds);
                    i += 4;
                    break;
                case SyneticaDataUpType.RangeHighAlarmCount:
                    result.RangeHighAlarmCount = new((ushort)((bytes[i + 1] << 8) | bytes[i + 2]));
                    i += 2;
                    break;
                case SyneticaDataUpType.PressureTx:
                    result.PressureTxMbar = new((ushort)((bytes[i + 1] << 8) | bytes[i + 2]), MeasurementUnit.Millibar);
                    i += 2;
                    break;
                case SyneticaDataUpType.TemperatureTx:
                    result.TemperatureTxDegC = new((short)((bytes[i + 1] << 8) | bytes[i + 2]) / 10f, MeasurementUnit.Celsius);
                    i += 2;
                    break;
                case SyneticaDataUpType.RangeAmpl:
                    result.RangeAmpl = new(((bytes[i + 1] << 8) | bytes[i + 2]) / 10f);
                    i += 2;
                    break;
                case SyneticaDataUpType.Co2e:
                    result.Co2ePpm = new(Unsafe.BitCast<int,float>((bytes[i + 2] << 24) | (bytes[i + 3] << 16) | (bytes[i + 4] << 8) | bytes[i + 5]), MeasurementUnit.PartsPerMillion);
                    i += 4;
                    break;
                case SyneticaDataUpType.SoundMin:
                    result.SoundMinDbA = new(Unsafe.BitCast<int,float>((bytes[i + 2] << 24) | (bytes[i + 3] << 16) | (bytes[i + 4] << 8) | bytes[i + 5]), MeasurementUnit.Decibels);
                    i += 4;
                    break;
                case SyneticaDataUpType.SoundAvg:
                    result.SoundAvgDbA = new(Unsafe.BitCast<int,float>((bytes[i + 2] << 24) | (bytes[i + 3] << 16) | (bytes[i + 4] << 8) | bytes[i + 5]), MeasurementUnit.Decibels);
                    i += 4;
                    break;
                case SyneticaDataUpType.SoundMax:
                    result.SoundMaxDbA = new(Unsafe.BitCast<int,float>((bytes[i + 2] << 24) | (bytes[i + 3] << 16) | (bytes[i + 4] << 8) | bytes[i + 5]), MeasurementUnit.Decibels);
                    i += 4;
                    break;
                case SyneticaDataUpType.CpuTemp:
                    result.CpuTemp = new(bytes[i + 1] + Math.Round(bytes[i + 2] * 100.0 / 256) / 100, MeasurementUnit.Celsius);
                    i += 2;
                    break;
                case SyneticaDataUpType.BattStatus:
                    result.BattStatus = new(bytes[i + 1]);
                    i += 1;
                    break;
                case SyneticaDataUpType.BattVolt:
                    result.BattVolt = new(((bytes[i + 1] << 8) | bytes[i + 2]) / 1000.0f, MeasurementUnit.Volts);
                    i += 2;
                    break;
                case SyneticaDataUpType.RxRssi:
                    result.RxRssi = new((short)((bytes[i + 1] << 8) | bytes[i + 2]));
                    i += 2;
                    break;
                case SyneticaDataUpType.RxSnr:
                    result.RxSnr = new((sbyte)bytes[i + 1]);
                    i += 1;
                    break;
                case SyneticaDataUpType.RxCount:
                    result.RxCount = new((ushort)((bytes[i + 1] << 8) | bytes[i + 2]));
                    i += 2;
                    break;
                case SyneticaDataUpType.TxTime:
                    result.TxTimeMs = new((ushort)((bytes[i + 1] << 8) | bytes[i + 2]), MeasurementUnit.MilliSeconds);
                    i += 2;
                    break;
                case SyneticaDataUpType.TxPower:
                    result.TxPowerDbm = new((sbyte)(bytes[i + 1]), MeasurementUnit.DecibelMilliwatts);
                    i += 1;
                    break;
                case SyneticaDataUpType.TxCount:
                    result.TxCount = new((ushort)((bytes[i + 1] << 8) | bytes[i + 2]));
                    i += 2;
                    break;
                case SyneticaDataUpType.PowerUpCount:
                    result.PowerUpCount = new((ushort)((bytes[i + 1] << 8) | bytes[i + 2]));
                    i += 2;
                    break;
                case SyneticaDataUpType.UsbInCount:
                    result.UsbInCount = new((ushort)((bytes[i + 1] << 8) | bytes[i + 2]));
                    i += 2;
                    break;
                case SyneticaDataUpType.LoginOkCount:
                    result.LoginOkCount = new((ushort)((bytes[i + 1] << 8) | bytes[i + 2]));
                    i += 2;
                    break;
                case SyneticaDataUpType.LoginFailCount:
                    result.LoginFailCount = new((ushort)((bytes[i + 1] << 8) | bytes[i + 2]));
                    i += 2;
                    break;
                case SyneticaDataUpType.FanRunTime:
                    result.FanRunTimeS = new((uint)(((bytes[i + 1] << 24) | (bytes[i + 2] << 16) | (bytes[i + 3] << 8) | bytes[i + 4])));
                    i += 4;
                    break;
                default:
                    i = bytes.Length; // something is wrong with data
                    break;
            }
        }

        return result;
    }

}
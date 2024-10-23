using Sensify.Extensions;

namespace Sensify.Decoders.Synetica;

public partial record SyneticaMeasurement
{
    public static SyneticaMeasurement operator +(SyneticaMeasurement lhs, SyneticaMeasurement rhs)
    {
        return new()
        {
            Temperature = lhs is null ? rhs?.Temperature : lhs.Temperature?.Add(rhs?.Temperature),
            Humidity = lhs is null ? rhs?.Humidity : lhs.Humidity?.Add(rhs?.Humidity),
            Lux = lhs is null ? rhs?.Lux : lhs.Lux?.Add(rhs?.Lux),
            PressureMbar = lhs is null ? rhs?.PressureMbar : lhs.PressureMbar?.Add(rhs?.PressureMbar),
            Iaq = lhs is null ? rhs?.Iaq : lhs.Iaq?.Add(rhs?.Iaq),
            O2Percentage = lhs is null ? rhs?.O2Percentage : lhs.O2Percentage?.Add(rhs?.O2Percentage),
            CarbonMonoxide = lhs is null ? rhs?.CarbonMonoxide : lhs.CarbonMonoxide?.Add(rhs?.CarbonMonoxide),
            CarbonDioxide = lhs is null ? rhs?.CarbonDioxide : lhs.CarbonDioxide?.Add(rhs?.CarbonDioxide),
            OzonePpm = lhs is null ? rhs?.OzonePpm : lhs.OzonePpm?.Add(rhs?.OzonePpm),
            OzonePpb = lhs is null ? rhs?.OzonePpb : lhs.OzonePpb?.Add(rhs?.OzonePpb),
            PollutantsKohm = lhs is null ? rhs?.PollutantsKohm : lhs.PollutantsKohm?.Add(rhs?.PollutantsKohm),
            Pm25 = lhs is null ? rhs?.Pm25 : lhs.Pm25?.Add(rhs?.Pm25),
            Pm10 = lhs is null ? rhs?.Pm10 : lhs.Pm10?.Add(rhs?.Pm10),
            HydrogenSulfide = lhs is null ? rhs?.HydrogenSulfide : lhs.HydrogenSulfide?.Add(rhs?.HydrogenSulfide),
            Counter = lhs is null ? rhs?.Counter : lhs.Counter.Add(rhs?.Counter),
            MbExceptions = lhs is null ? rhs?.MbExceptions : lhs.MbExceptions.Add(rhs?.MbExceptions),
            MbIntervalValues = lhs is null ? rhs?.MbIntervalValues : lhs.MbIntervalValues.Add(rhs?.MbIntervalValues),
            MbCumulativeValues = lhs is null ? rhs?.MbCumulativeValues : lhs.MbCumulativeValues.Add(rhs?.MbCumulativeValues),
            Bvoc = lhs is null ? rhs?.Bvoc : lhs.Bvoc?.Add(rhs?.Bvoc),
            PirDetectionCount = lhs is null ? rhs?.PirDetectionCount : lhs.PirDetectionCount?.Add(rhs?.PirDetectionCount),
            PirOccupiedTimeSeconds = lhs is null ? rhs?.PirOccupiedTimeSeconds : lhs.PirOccupiedTimeSeconds?.Add(rhs?.PirOccupiedTimeSeconds),
            TempProbe1 = lhs is null ? rhs?.TempProbe1 : lhs.TempProbe1?.Add(rhs?.TempProbe1),
            TempProbe2 = lhs is null ? rhs?.TempProbe2 : lhs.TempProbe2?.Add(rhs?.TempProbe2),
            TempProbe3 = lhs is null ? rhs?.TempProbe3 : lhs.TempProbe3?.Add(rhs?.TempProbe3),
            TempProbeInBandDurationS1 = lhs is null ? rhs?.TempProbeInBandDurationS1 : lhs.TempProbeInBandDurationS1?.Add(rhs?.TempProbeInBandDurationS1),
            TempProbeInBandDurationS2 = lhs is null ? rhs?.TempProbeInBandDurationS2 : lhs.TempProbeInBandDurationS2?.Add(rhs?.TempProbeInBandDurationS2),
            TempProbeHighAlarmCount3 = lhs is null ? rhs?.TempProbeHighAlarmCount3 : lhs.TempProbeHighAlarmCount3?.Add(rhs?.TempProbeHighAlarmCount3),
            DiffPressure = lhs is null ? rhs?.DiffPressure : lhs.DiffPressure?.Add(rhs?.DiffPressure),
            Voltage = lhs is null ? rhs?.Voltage : lhs.Voltage?.Add(rhs?.Voltage),
            Current = lhs is null ? rhs?.Current : lhs.Current?.Add(rhs?.Current),
            Resistance = lhs is null ? rhs?.Resistance : lhs.Resistance?.Add(rhs?.Resistance),
            LeakDetectEvent = lhs is null ? rhs?.LeakDetectEvent : lhs.LeakDetectEvent?.Add(rhs?.LeakDetectEvent),
            VibrationEvent = lhs is null ? rhs?.VibrationEvent : lhs.VibrationEvent?.Add(rhs?.VibrationEvent),
            RangeMm = lhs is null ? rhs?.RangeMm : lhs.RangeMm?.Add(rhs?.RangeMm),
            RangeInBandDurationS = lhs is null ? rhs?.RangeInBandDurationS : lhs.RangeInBandDurationS?.Add(rhs?.RangeInBandDurationS),
            RangeInBandAlarmCount = lhs is null ? rhs?.RangeInBandAlarmCount : lhs.RangeInBandAlarmCount?.Add(rhs?.RangeInBandAlarmCount),
            RangeLowDurationS = lhs is null ? rhs?.RangeLowDurationS : lhs.RangeLowDurationS?.Add(rhs?.RangeLowDurationS),
            RangeLowAlarmCount = lhs is null ? rhs?.RangeLowAlarmCount : lhs.RangeLowAlarmCount?.Add(rhs?.RangeLowAlarmCount),
            RangeHighDurationS = lhs is null ? rhs?.RangeHighDurationS : lhs.RangeHighDurationS?.Add(rhs?.RangeHighDurationS),
            RangeHighAlarmCount = lhs is null ? rhs?.RangeHighAlarmCount : lhs.RangeHighAlarmCount?.Add(rhs?.RangeHighAlarmCount),
            PressureTxMbar = lhs is null ? rhs?.PressureTxMbar : lhs.PressureTxMbar?.Add(rhs?.PressureTxMbar),
            TemperatureTxDegC = lhs is null ? rhs?.TemperatureTxDegC : lhs.TemperatureTxDegC?.Add(rhs?.TemperatureTxDegC),
            RangeAmpl = lhs is null ? rhs?.RangeAmpl : lhs.RangeAmpl?.Add(rhs?.RangeAmpl),
            Co2ePpm = lhs is null ? rhs?.Co2ePpm : lhs.Co2ePpm?.Add(rhs?.Co2ePpm),
            SoundMinDbA = lhs is null ? rhs?.SoundMinDbA : lhs.SoundMinDbA?.Add(rhs?.SoundMinDbA),
            SoundAvgDbA = lhs is null ? rhs?.SoundAvgDbA : lhs.SoundAvgDbA?.Add(rhs?.SoundAvgDbA),
            SoundMaxDbA = lhs is null ? rhs?.SoundMaxDbA : lhs.SoundMaxDbA?.Add(rhs?.SoundMaxDbA),
            CpuTemp = lhs is null ? rhs?.CpuTemp : lhs.CpuTemp?.Add(rhs?.CpuTemp),
            BattVolt = lhs is null ? rhs?.BattVolt : lhs.BattVolt?.Add(rhs?.BattVolt),
            RxRssi = lhs is null ? rhs?.RxRssi : lhs.RxRssi?.Add(rhs?.RxRssi),
            RxSnr = lhs is null ? rhs?.RxSnr : lhs.RxSnr?.Add(rhs?.RxSnr),
            RxCount = lhs is null ? rhs?.RxCount : lhs.RxCount?.Add(rhs?.RxCount),
            TxTimeMs = lhs is null ? rhs?.TxTimeMs : lhs.TxTimeMs?.Add(rhs?.TxTimeMs),
            TxPowerDbm = lhs is null ? rhs?.TxPowerDbm : lhs.TxPowerDbm?.Add(rhs?.TxPowerDbm),
            TxCount = lhs is null ? rhs?.TxCount : lhs.TxCount?.Add(rhs?.TxCount),
            PowerUpCount = lhs is null ? rhs?.PowerUpCount : lhs.PowerUpCount?.Add(rhs?.PowerUpCount),
            UsbInCount = lhs is null ? rhs?.UsbInCount : lhs.UsbInCount?.Add(rhs?.UsbInCount),
            LoginOkCount = lhs is null ? rhs?.LoginOkCount : lhs.LoginOkCount?.Add(rhs?.LoginOkCount),
            LoginFailCount = lhs is null ? rhs?.LoginFailCount : lhs.LoginFailCount?.Add(rhs?.LoginFailCount),
            FanRunTimeS = lhs is null ? rhs?.FanRunTimeS : lhs.FanRunTimeS?.Add(rhs?.FanRunTimeS)
        };
    }

}
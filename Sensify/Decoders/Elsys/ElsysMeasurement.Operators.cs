using Sensify.Extensions;

namespace Sensify.Decoders.Elsys;
public partial record ElsysMeasurement
{
    public static ElsysMeasurement operator +(ElsysMeasurement lhs, ElsysMeasurement rhs)
    {
        return new ElsysMeasurement
        {
             Temperature = lhs is null ? rhs?.Temperature : lhs.Temperature.Add(rhs?.Temperature), 
             Humidity = lhs is null ? rhs?.Humidity : lhs.Humidity.Add(rhs?.Humidity),
             Acceleration = lhs is null ? rhs?.Acceleration : lhs.Acceleration.Add(rhs?.Acceleration),
             Ligth = lhs is null ? rhs?.Ligth : lhs.Ligth.Add(rhs?.Ligth),
             Motion = lhs is null ? rhs?.Motion : lhs.Motion.Add(rhs?.Motion),
             Co2 = lhs is null ? rhs?.Co2 : lhs.Co2.Add(rhs?.Co2),
             Vdd = lhs is null ? rhs?.Vdd : lhs.Vdd.Add(rhs?.Vdd),
             Analog1 = lhs is null ? rhs?.Analog1 : lhs.Analog1.Add(rhs?.Analog1),
             Gps = lhs is null ? rhs?.Gps : lhs.Gps.Add(rhs?.Gps),
             Pulse1 = lhs is null ? rhs?.Pulse1 : lhs.Pulse1.Add(rhs?.Pulse1),
             Pulse1Absolute = lhs is null ? rhs?.Pulse1Absolute : lhs.Pulse1Absolute.Add(rhs?.Pulse1Absolute),
             ExternalTemperature = lhs is null ? rhs?.ExternalTemperature : lhs.ExternalTemperature.Add(rhs?.ExternalTemperature),
             Digital = lhs is null ? rhs?.Digital : lhs.Digital.Add(rhs?.Digital),
             Distance = lhs is null ? rhs?.Distance : lhs.Distance.Add(rhs?.Distance),
             AccelerationMotion = lhs is null ? rhs?.AccelerationMotion : lhs.AccelerationMotion.Add(rhs?.AccelerationMotion),
             InfraRedInternalTemperature = lhs is null ? rhs?.InfraRedInternalTemperature : lhs.InfraRedInternalTemperature.Add(rhs?.InfraRedInternalTemperature),
             InfraRedExternalTemperature = lhs is null ? rhs?.InfraRedExternalTemperature : lhs.InfraRedExternalTemperature.Add(rhs?.InfraRedExternalTemperature),
             Occupancy = lhs is null ? rhs?.Occupancy : lhs.Occupancy.Add(rhs?.Occupancy),
             WaterLeak = lhs is null ? rhs?.WaterLeak : lhs.WaterLeak.Add(rhs?.WaterLeak),
             Grideye = lhs is null ? rhs?.Grideye : lhs.Grideye.Add(rhs?.Grideye),
             Pressure = lhs is null ? rhs?.Pressure : lhs.Pressure.Add(rhs?.Pressure),
             SoundPeak = lhs is null ? rhs?.SoundPeak : lhs.SoundPeak.Add(rhs?.SoundPeak),
             SoundAverage = lhs is null ? rhs?.SoundAverage : lhs.SoundAverage.Add(rhs?.SoundAverage),
             Pulse2 = lhs is null ? rhs?.Pulse2 : lhs.Pulse2.Add(rhs?.Pulse2),
             PulseAbsolute2 = lhs is null ? rhs?.PulseAbsolute2 : lhs.PulseAbsolute2.Add(rhs?.PulseAbsolute2),
             Analog2 = lhs is null ? rhs?.Analog2 : lhs.Analog2.Add(rhs?.Analog2),
             ExternalTemperature2 = lhs is null ? rhs?.ExternalTemperature2 : lhs.ExternalTemperature2.Add(rhs?.ExternalTemperature2),
             Digital2 = lhs is null ? rhs?.Digital2 : lhs.Digital2.Add(rhs?.Digital2),
             AnalogUv = lhs is null ? rhs?.AnalogUv : lhs.AnalogUv.Add(rhs?.AnalogUv),
             Tvoc = lhs is null ? rhs?.Tvoc : lhs.Tvoc.Add(rhs?.Tvoc)
        };

    }
}

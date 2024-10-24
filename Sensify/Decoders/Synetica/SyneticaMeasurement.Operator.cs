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
            AmbientLight = lhs is null ? rhs?.AmbientLight : lhs.AmbientLight?.Add(rhs?.AmbientLight),
            Pressure = lhs is null ? rhs?.Pressure : lhs.Pressure?.Add(rhs?.Pressure),
            VolatileOrganicCompounds = lhs is null ? rhs?.VolatileOrganicCompounds : lhs.VolatileOrganicCompounds?.Add(rhs?.VolatileOrganicCompounds),
            Bvoc = lhs is null ? rhs?.Bvoc : lhs.Bvoc?.Add(rhs?.Bvoc),
            Co2ePpm = lhs is null ? rhs?.Co2ePpm : lhs.Co2ePpm?.Add(rhs?.Co2ePpm),
            SoundMin = lhs is null ? rhs?.SoundMin : lhs.SoundMin?.Add(rhs?.SoundMin),
            SoundAvg = lhs is null ? rhs?.SoundAvg : lhs.SoundAvg?.Add(rhs?.SoundAvg),
            SoundMax = lhs is null ? rhs?.SoundMax : lhs.SoundMax?.Add(rhs?.SoundMax),
            BattVolt = lhs is null ? rhs?.BattVolt : lhs.BattVolt?.Add(rhs?.BattVolt)
        };
    }

}
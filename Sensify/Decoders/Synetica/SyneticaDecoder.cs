using Sensify.Decoders.Common;
using Sensify.Extensions;

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
                case SyneticaDataUpType.Temperature:

                    result.Temperature = new((short)((bytes[i + 1] << 8) | (bytes[i + 2])) / 10f, MeasurementUnit.Celsius);
                    i += 2;
                    break;

                case SyneticaDataUpType.Humidity:
                    result.Humidity = new(bytes[i + 1], MeasurementUnit.Percentage);
                    i += 1;
                    break;

                case SyneticaDataUpType.AmbientLight:
                    result.AmbientLight = new((ushort)((bytes[i + 1] << 8) | bytes[i + 2]), MeasurementUnit.Lux);
                    i += 2;
                    break;
                case SyneticaDataUpType.Pressure:
                    result.Pressure = new((ushort)((bytes[i + 1] << 8) | bytes[i + 2]), MeasurementUnit.Millibar);
                    i += 2;
                    break;
                case SyneticaDataUpType.VolatileOrganicCompounds:
                    result.VolatileOrganicCompounds = new((ushort)((bytes[i + 1] << 8) | bytes[i + 2]));
                    i += 2;
                    break;
                case SyneticaDataUpType.Voc:
                    result.Bvoc = new(bytes[(i + 1)..].BitCastAsFloat(), MeasurementUnit.PartsPerMillion);
                    i += 4;
                    break;
                case SyneticaDataUpType.Co2e:
                    result.Co2e = new(bytes[(i + 1)..].BitCastAsFloat(), MeasurementUnit.PartsPerMillion);
                    i += 4;
                    break;
                case SyneticaDataUpType.SoundMin:
                    result.SoundMin = new(bytes[(i + 1)..].BitCastAsFloat(), MeasurementUnit.Decibels);
                    i += 4;
                    break;
                case SyneticaDataUpType.SoundAvg:
                    result.SoundAvg = new(bytes[(i + 1)..].BitCastAsFloat(), MeasurementUnit.Decibels);
                    i += 4;
                    break;
                case SyneticaDataUpType.SoundMax:
                    result.SoundMax = new(bytes[(i + 1)..].BitCastAsFloat(), MeasurementUnit.Decibels);
                    i += 4;
                    break;

                case SyneticaDataUpType.BattVolt:
                    result.BattVolt = new(((ushort)((bytes[i + 1] << 8) | bytes[i + 2])) / 1000f, MeasurementUnit.Volts);
                    i += 2;
                    break;

                default:
                    i = bytes.Length; // something is wrong with data or end of data
                    break;
            }
        }

        return result;
    }

}
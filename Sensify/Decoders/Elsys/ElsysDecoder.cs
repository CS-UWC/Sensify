/*
______ _       _______     _______ 
|  ____| |     / ____\ \   / / ____|
| |__  | |    | (___  \ \_/ / (___  
|  __| | |     \___ \  \   / \___ \ 
| |____| |____ ____) |  | |  ____) |
|______|______|_____/   |_| |_____/ 

ELSYS simple payload decoder. 
Use it as it is or remove the bugs :)
www.elsys.se
peter@elsys.se

this code is an adaptation of the javascript elsys decoder
*/


using Sensify.Decoders.Common;
using Sensify.Extensions;

namespace Sensify.Decoders.Elsys;

public sealed class ElsysDecoder
{
    public ElsysMeasurement Decode(string payload) => Decode(payload.AsSpan());

    public ElsysMeasurement Decode(ReadOnlySpan<char> payload)
    {
        var bytes = payload.ToHexBytes();

        var result = new ElsysMeasurement();

        for (var i = 0; i < bytes.Length; i++)
        {

            switch ((ElsysDataUpType)bytes[i])
            {
                case ElsysDataUpType.Temp:
                    result = result with
                    {
                        Temperature = new((short)(bytes[i + 1] << 8 | bytes[i + 2]) / 10f, MeasurementUnit.Celsius)
                    };
                    i += 2;
                    break;

                case ElsysDataUpType.Rh:
                    result = result with
                    {
                        Humidity = new(bytes[i + 1], MeasurementUnit.Percentage)
                    };
                    i++;
                    break;

                case ElsysDataUpType.Acc:

                    result = result with
                    {
                        Acceleration = new(new((sbyte)bytes[i + 1], (sbyte)bytes[i + 2], (sbyte)bytes[i + 3]), MeasurementUnit.GForce)
                    };

                    i += 3;
                    break;
                case ElsysDataUpType.Light:
                    result = result with
                    {
                        Light = new((ushort)(bytes[i + 1] << 8 | bytes[i + 2]), MeasurementUnit.Lux)
                    };
                    i += 2;
                    break;
                case ElsysDataUpType.Motion:
                    result = result with
                    {
                        Motion = new(bytes[i + 1])
                    };
                    i++;
                    break;
                case ElsysDataUpType.Co2:
                    result = result with
                    {
                        Co2 = new((ushort)(bytes[i + 1] << 8 | bytes[i + 2]), MeasurementUnit.PartsPerBillion)
                    };
                    i += 2;
                    break;
                case ElsysDataUpType.Vdd:
                    result = result with
                    {
                        Vdd = new((ushort)(bytes[i + 1] << 8 | bytes[i + 2]), MeasurementUnit.MilliVolts)
                    };
                    i += 2;
                    break;
                case ElsysDataUpType.Pulse1Abs:
                    result = result with
                    {
                        Pulse1Absolute = new((uint)(bytes[i + 1] << 24 | bytes[i + 2] << 16 | bytes[i + 3] << 8 | bytes[i + 4]))
                    };
                    i += 4;
                    break;
                case ElsysDataUpType.ExtDigital:
                    result = result with
                    {
                        Digital = new(bytes[i + 1])
                    };
                    i += 1;
                    break;
                case ElsysDataUpType.AccMotion:
                    result = result with
                    {
                        AccelerationMotion = new(bytes[i + 1])
                    };
                    i += 1;
                    break;

                default:
                    i = bytes.Length; // something is wrong with data, or unsupported sensors
                    break;
            }
        }

        return result;

    }
}

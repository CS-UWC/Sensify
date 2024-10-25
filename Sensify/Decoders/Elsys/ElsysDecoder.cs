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
                case ElsysDataUpType.Analog1:
                    result = result with
                    {
                        Analog1 = new((ushort)(bytes[i + 1] << 8 | bytes[i + 2]), MeasurementUnit.MilliVolts)
                    };
                    i += 2;
                    break;
                case ElsysDataUpType.Gps:
                    result = result with
                    {
                        Gps = new(new((bytes[i] | bytes[i + 1] << 8 | bytes[i + 2] << 16 | ((bytes[i + 2] & 0x80) == 0x80 ? 0xFF << 24 : 0)) / 10_000.0f,
                        (bytes[i + 3] | bytes[i + 4] << 8 | bytes[i + 5] << 16 | ((bytes[i + 5] & 0x80) == 0x80 ? 0xFF << 24 : 0)) / 10_000.0f))
                    };
                    i += 5;
                    break;
                case ElsysDataUpType.Pulse1:
                    result = result with
                    {
                        Pulse1 = new((ushort)(bytes[i + 1] << 8 | bytes[i + 2]))
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
                case ElsysDataUpType.ExtTemp1:
                    result = result with
                    {
                        ExternalTemperature = new((short)(bytes[i + 1] << 8 | bytes[i + 2]) / 10f, MeasurementUnit.Celsius)
                    };
                    i += 2;
                    break;
                case ElsysDataUpType.ExtDigital:
                    result = result with
                    {
                        Digital = new(bytes[i + 1])
                    };
                    i += 1;
                    break;
                case ElsysDataUpType.ExtDistance:
                    result = result with
                    {
                        Distance = new((ushort)(bytes[i + 1] << 8 | bytes[i + 2]), MeasurementUnit.Millimeters)
                    };
                    i += 2;
                    break;
                case ElsysDataUpType.AccMotion:
                    result = result with
                    {
                        AccelerationMotion = new(bytes[i + 1])
                    };
                    i += 1;
                    break;

                case ElsysDataUpType.IrTemp:
                    result = result with
                    {
                        InfraRedInternalTemperature = new((short)(bytes[i + 1] << 8 | bytes[i + 2]) / 10f, MeasurementUnit.Celsius),
                        InfraRedExternalTemperature = new((short)(bytes[i + 3] << 8 | bytes[i + 4]) / 10f, MeasurementUnit.Celsius)
                    };
                    i += 4;
                    break;

                case ElsysDataUpType.Occupancy:
                    result = result with
                    {
                        Occupancy = new(bytes[i + 1])
                    };
                    i += 1;
                    break;

                case ElsysDataUpType.WaterLeak:
                    result = result with
                    {
                        WaterLeak = new(bytes[i + 1])
                    };
                    i += 1;
                    break;
                case ElsysDataUpType.GridEye:
                    var @ref = bytes[i + 1];
                    i++;
                    float[] grideye = new float[64];

                    for (int j = 0; j < 64; j++)
                    {
                        grideye[j] = (@ref + bytes[1 + i + j]) / 10f;
                    }

                    result = result with
                    {
                        Grideye = new(grideye, MeasurementUnit.Celsius)
                    };

                    i += 64;
                    break;
                case ElsysDataUpType.Pressure:
                    result = result with
                    {
                        Pressure = new((bytes[i + 1] << 24 | bytes[i + 2] << 16 | bytes[i + 3] << 8 | bytes[i + 4]) / 1000f, MeasurementUnit.Hectopascal)
                    };
                    i += 4;
                    break;
                case ElsysDataUpType.Sound:
                    result = result with
                    {
                        SoundPeak = new(bytes[i + 1]),
                        SoundAverage = new(bytes[i + 2]),
                    };
                    i += 2;
                    break;
                case ElsysDataUpType.Pulse2:
                    result = result with
                    {
                        Pulse2 = new((ushort)(bytes[i + 1] << 8 | bytes[i + 2]))
                    };
                    i += 2;
                    break;
                case ElsysDataUpType.Pulse2Abs:
                    result = result with
                    {
                        PulseAbsolute2 = new((uint)(bytes[i + 1] << 24 | bytes[i + 2] << 16 | bytes[i + 3] << 8 | bytes[i + 4]))
                    };
                    i += 4;
                    break;
                case ElsysDataUpType.Analog2:
                    result = result with
                    {
                        Analog2 = new((ushort)(bytes[i + 1] << 8 | bytes[i + 2]))
                    };
                    i += 2;
                    break;
                case ElsysDataUpType.ExtTemp2:

                    var externalTemperature2 = result.ExternalTemperature2?.Value;

                    if (externalTemperature2 is null)
                    {
                        externalTemperature2 = [];
                        result = result with { ExternalTemperature2 = new(externalTemperature2) };
                    }

                    externalTemperature2.Add((short)(bytes[i + 1] << 8 | bytes[i + 2]) / 10f);
                    i += 2;
                    break;
                case ElsysDataUpType.ExtDigital2:
                    result = result with
                    {
                        Digital2 = new(bytes[i + 1])
                    };
                    i += 1;
                    break;
                case ElsysDataUpType.ExtAnalogUV:
                    result = result with
                    {
                        AnalogUv = new(bytes[i + 1] << 24 | bytes[i + 2] << 16 | bytes[i + 3] << 8 | bytes[i + 4], MeasurementUnit.UV)
                    };
                    i += 4;
                    break;
                case ElsysDataUpType.TVOC:
                    result = result with
                    {
                        Tvoc = new((ushort)(bytes[i + 1] << 8 | bytes[i + 2]))
                    };
                    i += 2;
                    break;

                default:
                    i = bytes.Length; // something is wrong with data 
                    break;
            }
        }

        return result;

    }
}

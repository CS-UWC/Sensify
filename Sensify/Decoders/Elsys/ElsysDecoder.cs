using Sensify.Extensions;

namespace Sensify.Decoders.Elsys;


public sealed class ElsysDecoder 
{
    public IDictionary<string, object> Decode(string payload) => Decode(payload.AsSpan());

    public IDictionary<string, object> Decode(ReadOnlySpan<char> payload){
        var bytes = payload.ToHexBytes();

        var result = new Dictionary<string, object>();

        for(var i = 0; i < bytes.Length; i++){

            switch((ElsysTDataUpType)bytes[i]){
                case ElsysTDataUpType.Temp:
                    result["temperature"] = new {value = (short)((bytes[i+1] << 8) | bytes[i+2]) / 10f, unit = "C"};
                    i+=2;
                    break;

                case ElsysTDataUpType.Rh:
                    result["humidity"] = new {value = bytes[i+1], unit = "%"};
                    i++;
                    break;

                case ElsysTDataUpType.Acc:
                    result["x"] = new {value = (sbyte)bytes[i+1], unit = ""};
                    result["y"] = new {value = (sbyte)bytes[i+2], unit = ""};
                    result["z"] = new {value = (sbyte)bytes[i+3], unit = ""};
                    
                    i += 3;
                    break;
                case ElsysTDataUpType.Light:
                    result["light"] = new {value = (ushort)((bytes[i+1] << 8) | bytes[i+2]), unit = "lux"};
                    i+= 2;
                    break;
                case ElsysTDataUpType.Motion:
                    result["motion"] = new {value = bytes[i+1], unit = ""};
                    i++;
                    break;
                case ElsysTDataUpType.Co2:
                    result["co2"] = new {value = (ushort)((bytes[i+1] << 8) | bytes[i+2]), unit = "ppm"};
                    i+= 2;
                    break;
                case ElsysTDataUpType.Vdd:
                    result["vdd"] = new {value = (ushort)((bytes[i+1] << 8) | bytes[i+2]), unit = "mV"};
                    i+= 2;
                    break;
                case ElsysTDataUpType.Analog1:
                    result["analog1"] = new {value = (ushort)((bytes[i+1] << 8) | bytes[i+2]), unit = "mV"};
                    i+= 2;
                    break;
                case ElsysTDataUpType.Gps:
                    result["lat"] = (bytes[i] | bytes[i+1] << 8 | bytes[i+2] << 16 | ((bytes[i+2] & 0x80) == 0x80 ? 0xFF << 24 : 0)) / 10_000.0f; 
                    result["long"] = (bytes[i+3] | bytes[i+4] << 8 | bytes[i+5] << 16 | ((bytes[i+5] & 0x80) == 0x80 ? 0xFF << 24 : 0)) / 10_000.0f; 
                    i += 5;
                    break;
                case ElsysTDataUpType.Pulse1:
                    result["pulse1"] = (bytes[i+1] << 8) | (bytes[i+2]);
                    i+= 2;
                    break;
                case ElsysTDataUpType.Pulse1Abs:
                    result["pulse1"] = (uint)((bytes[i+1] << 24) | (bytes[i+2] << 16) | (bytes[i+3] << 8) | bytes[i+4]);
                    i+= 4;
                    break;
                case ElsysTDataUpType.ExtTemp1:
                    result["externalTemperature"] = (short)((bytes[i+1] << 8) | (bytes[i+2]))/10f;
                    i+= 2;
                    break;
                case ElsysTDataUpType.ExtDigital:
                    result["digital"] = bytes[i + 1];
                    i+= 1;
                    break;
                case ElsysTDataUpType.ExtDistance:
                    result["distance"] = (bytes[i+1] << 8) | bytes[i+2];
                    i+= 2;
                    break;
                case ElsysTDataUpType.AccMotion:
                    result["accMotion"] = bytes[i+1];
                    i+= 1;
                    break;


                default: break;

            }
        }

        return result;

    }
}

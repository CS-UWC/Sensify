using Sensify.Decoders.Common;
using Sensify.Extensions;
using System.Runtime.CompilerServices;

namespace Sensify.Decoders.Netvox;

public sealed class NetvoxDecoder
{
    public NetvoxMeasurement? Decode(string payload) => Decode(payload.AsSpan());

    public NetvoxMeasurement? Decode(ReadOnlySpan<char> payload)
    {

        // first byte is device version
        // second byte is device type
        // 
        var data = payload.ToHexBytes();

        return (NetvoxDeviceType)data[1] switch
        {
            >= NetvoxDeviceType.R718CK2 and <= NetvoxDeviceType.R718CN2_R718CR2 => DecodeR718CK2ToR718CN2_R718CR2(data[2..]),
            NetvoxDeviceType.R718E => DecodeR718E(data[2..]),
            NetvoxDeviceType.R311A => DecodeR311A(data[2..]),
            _ => null
        };
    }

    public static NetvoxMeasurement? DecodeR718E(ReadOnlySpan<byte> data)
    {
        var result = new NetvoxMeasurement();

        switch ((NetvoxDataUpType)data[0])
        {
            case NetvoxDataUpType.R718E_Acceleration:

                var battery = data[1] * 0.1f; // volts
                var accelerationX = Unsafe.BitCast<int, float>(data[3] << 24 | data[2] << 16); // m/s2
                var accelerationY = Unsafe.BitCast<int, float>(data[5] << 24 | data[4] << 16); // m/s2
                var accelerationZ = Unsafe.BitCast<int, float>(data[7] << 24 | data[6] << 16); // m/s2

                result.Battery = new(battery, MeasurementUnit.Volts);
                result.Acceleration = new(new(accelerationX, accelerationY, accelerationZ), MeasurementUnit.MetersPerSecond2);

                break;

            case NetvoxDataUpType.R718E_Velocity:

                var velocityX = Unsafe.BitCast<int, float>(data[2] << 24 | data[1] << 16); // mm/s
                var velocityY = Unsafe.BitCast<int, float>(data[4] << 24 | data[3] << 16); // mm/s
                var velocityZ = Unsafe.BitCast<int, float>(data[6] << 24 | data[5] << 16); // mm/s
                var temperature = (short)(data[7] << 8 | data[8]) * 0.1f; // Celsius

                result.Velocity = new(new(velocityX, velocityY, velocityZ), MeasurementUnit.MillimetersPerSecond);
                result.Temperature = new(temperature, MeasurementUnit.Celsius);

                break;

            default:
                return null;
        }

        return result;

    }

    private static NetvoxMeasurement? DecodeR718CK2ToR718CN2_R718CR2(ReadOnlySpan<byte> data)
    {

        var result = new NetvoxMeasurement();

        switch ((NetvoxDataUpType)data[0])
        {
            case NetvoxDataUpType.R718CK2_Temperature:

                var battery = data[1] * 0.1f; // volts
                var temperature1 = ((short)((data[2] << 8) | data[3])) * 0.1f; // celsius
                var temperature2 = ((short)((data[4] << 8) | data[5])) * 0.1f; // celsius
                result.Battery = new(battery, MeasurementUnit.Volts);
                result.Temperature1 = new(temperature1, MeasurementUnit.Celsius);
                result.Temperature2 = new(temperature2, MeasurementUnit.Celsius);

                break;
            default:
                return null;
        }

        return result;
    }
    
    private static NetvoxMeasurement? DecodeR311A(ReadOnlySpan<byte> data)
    {

        var result = new NetvoxMeasurement();

        switch ((NetvoxDataUpType)data[0])
        {
            case NetvoxDataUpType.R311A_Contact:

                var battery = data[1] * 0.1f; // volts
                var contact = data[2] == 1 ? Switch.On : Switch.Off;
                result.Battery = new(battery, MeasurementUnit.Volts);
                result.Contact = new(contact);

                break;
            default:
                return null;
        }

        return result;
    }

}

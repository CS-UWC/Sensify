using EnumToStringGenerator;
using System.Text.Json.Serialization;

namespace Sensify.Decoders.Common;

[GenerateStrings]
[JsonConverter(typeof(JsonConverterMeasurementUnit))]
public enum MeasurementUnit
{
    None,
    Celsius,
    Percentage,
    GForce,
    Lux,
    MilliVolts,
    Millimeters,
    Hectopascal,
    UV,
    PartsPerBillion

}
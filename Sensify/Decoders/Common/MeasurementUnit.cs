using EnumToStringGenerator;
using System.Text.Json.Serialization;

namespace Sensify.Decoders.Common;

[GenerateStrings]
[JsonConverter(typeof(JsonConverterMeasurementUnit))]
public enum MeasurementUnit
{
    [JsonPropertyName("°C")]
    None,
    [JsonPropertyName("°C")]
    Celsius,
    [JsonPropertyName("%")]
    Percentage,
    GForce,
    Lux,
    [JsonPropertyName("V")]
    Volts,
    [JsonPropertyName("mV")]
    MilliVolts,
    [JsonPropertyName("m")]
    Meters,
    [JsonPropertyName("mm")]
    Millimeters,
    [JsonPropertyName("mm/s")]
    MillimetersPerSecond,
    [JsonPropertyName("m/s^2")]
    MetersPerSecond2,
    [JsonPropertyName("hPa")]
    Hectopascal,
    [JsonPropertyName("uV")]
    UV,
    [JsonPropertyName("Ppm")]
    PartsPerMillion,
    [JsonPropertyName("Ppb")]
    PartsPerBillion,
    Millibar,
    IaqIndex,
    UgPerM3,
    Count,
    MilliSeconds,
    Seconds,
    Decibels,
    DecibelMilliwatts,
    MicroVolts,
    KiloOhms,
    MicrogramsPerCubicMeter,
    MilliAmperes,
    Amperes,
    Pascal

}
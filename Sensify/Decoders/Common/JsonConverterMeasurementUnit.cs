using Sensify.Extensions;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Sensify.Decoders.Common;

internal sealed class JsonConverterMeasurementUnit : JsonConverter<MeasurementUnit>
{

    public override MeasurementUnit Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var currentType = reader.TokenType;

        if(currentType is not JsonTokenType.Number or JsonTokenType.String)
        {
            throw new ArgumentOutOfRangeException(currentType.ToString());
        }

        if (currentType is JsonTokenType.Number) return (MeasurementUnit)reader.GetInt64();

        var value = reader.GetString();

        ArgumentNullException.ThrowIfNullOrWhiteSpace(value);

        if (long.TryParse(value, out var l)) return (MeasurementUnit)l;

        return value.AsMeasurementUnit();
    }

    public override void Write(Utf8JsonWriter writer, MeasurementUnit value, JsonSerializerOptions options)
    {

        writer.WriteStringValue(value.AsString());
    }
}

internal sealed class JsonConverterUnixDateTime : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DateTimeOffset.FromUnixTimeMilliseconds(reader.GetInt64()).UtcDateTime;
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(new DateTimeOffset(value).ToUnixTimeMilliseconds());
    }
}
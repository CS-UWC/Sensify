using Sensify.Decoders.Common;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sensify.Grains.Sensors.Common;

public sealed class JsonConverterNullableSensorId : JsonConverter<SensorId?>
{
    public override SensorId? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var currentType = reader.TokenType;

        if (currentType is not JsonTokenType.String) throw new InvalidSensorIdJsonException(JsonTokenType.String, currentType);

        var value = reader.GetString();

        return SensorId.IsValid(value) ? SensorId.From(value) : default;
    }

    public override void Write(Utf8JsonWriter writer, SensorId? value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public sealed class InvalidSensorIdJsonException(JsonTokenType expected, JsonTokenType found) 
        : Exception($"expected token type {expected}, but found {found}.");
}
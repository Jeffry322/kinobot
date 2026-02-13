using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace KinoBot.API.Common.Enums;

[JsonConverter(typeof(MediaTypeConverter))]
public enum MediaType
{
    [EnumMember(Value = "movie")]
    Movie,
    [EnumMember(Value = "tv")]
    TvShow,
    [EnumMember(Value = "person")]
    Person,
}

public class MediaTypeConverter : JsonConverter<MediaType>
{
    public override MediaType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return value switch
        {
            "movie" => MediaType.Movie,
            "tv" => MediaType.TvShow,
            "person" => MediaType.Person,
            _ => throw new JsonException($"Unknown media type: {value}")
        };
    }

    public override void Write(Utf8JsonWriter writer, MediaType value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value switch
        {
            MediaType.Movie => "movie",
            MediaType.TvShow => "tv",
            MediaType.Person => "person",
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        });
    }
}
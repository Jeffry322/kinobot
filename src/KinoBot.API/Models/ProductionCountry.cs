using System.Text.Json.Serialization;

namespace KinoBot.API.Models;

public sealed class ProductionCountry
{
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("iso_3166_1")]
    public string Code { get; init; } = string.Empty;
}
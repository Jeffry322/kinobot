using System.Text.Json.Serialization;

namespace KinoBot.API.Models;

public sealed class ProductionCompany
{
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;
}
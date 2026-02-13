using System.Text.Json.Serialization;

namespace KinoBot.API.Models;

public sealed class CreatedBy
{
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;
}
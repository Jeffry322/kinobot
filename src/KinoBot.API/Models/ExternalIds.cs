using System.Text.Json.Serialization;

namespace KinoBot.API.Models;

public sealed class ExternalIds
{
    [JsonPropertyName("imdb_id")]
    public string? ImdbId { get; init; }
}
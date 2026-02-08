using System.Text.Json.Serialization;

namespace KinoBot.API.Models;

public sealed class Genre
{
  [JsonPropertyName("name")] public required string Name { get; init; }
}
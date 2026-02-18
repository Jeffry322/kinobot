using System.Text.Json.Serialization;

namespace KinoBot.API.Models;

public sealed class Person
{
    [JsonPropertyName("adult")] public bool Adult { get; set; }
    [JsonPropertyName("gender")] public string Gender { get; set; } = string.Empty;
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("known_for_department")] public string KnownForDepartment { get; set; } = string.Empty;
    [JsonPropertyName("name")] public string Name { get; set; } = string.Empty;
    [JsonPropertyName("original_name")] public string OriginalName { get; set; } = string.Empty;
    [JsonPropertyName("popularity")] public float Popularity { get; set; }
    [JsonPropertyName("profile_path")] public string ProfilePath { get; set; } = string.Empty;
    [JsonPropertyName("cast_id")] public int CastId { get; set; }
    [JsonPropertyName("character")] public string Character { get; set; } = string.Empty;
    [JsonPropertyName("credit_id")] public string CreditId { get; set; } = string.Empty;
    [JsonPropertyName("order")] public int Order { get; set; }
}
using System.Text.Json.Serialization;

namespace KinoBot.API.Models;

public class SearchResult
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("media_type")]
    public required string MediaType { get; init; } = string.Empty;

    [JsonPropertyName("title")]
    public string? Title { get; init; }

    [JsonPropertyName("original_title")]
    public string? OriginalTitle { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("original_name")]
    public string? OriginalName { get; init; }

    [JsonPropertyName("release_date")]
    public string? ReleaseDate { get; init; }

    [JsonPropertyName("first_air_date")]
    public string? FirstAirDate { get; init; }

    [JsonPropertyName("poster_path")]
    public string? PosterPath { get; init; }

    [JsonPropertyName("vote_average")]
    public double? VoteAverage { get; init; }

    [JsonPropertyName("popularity")]
    public double Popularity { get; init; }

    public string DisplayName => Title ?? Name ?? "Unknown";
    public string DisplayOriginalName => OriginalTitle ?? OriginalName ?? "Unknown";

    public string DisplayDate
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(ReleaseDate))
                return ReleaseDate;
            if (!string.IsNullOrWhiteSpace(FirstAirDate))
                return FirstAirDate;
            return "Unknown";
        }
    }

    public string MediaTypeDisplay
    {
        get
        {
            var mt = MediaType?.ToLowerInvariant();
            return mt switch
            {
                "movie" => "movie",
                "tv" => "series",
                "person" => "person",
                _ => "unknown"
            };
        }
    }
}
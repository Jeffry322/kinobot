using System.Text.Json.Serialization;

namespace KinoBot.API.Models;

public class Movie
{
    [JsonPropertyName("id")] public int Id { get; init; }

    [JsonPropertyName("imdb_id")] public string? ImdbId { get; init; }

    [JsonPropertyName("title")] public required string Title { get; init; }

    [JsonPropertyName("tagline")] public string? Tagline { get; init; }
    
    [JsonPropertyName("overview")] public string? Overview { get; init; }

    [JsonPropertyName("release_date")] public string? ReleaseDate { get; init; }

    [JsonPropertyName("poster_path")] public string? PosterPath { get; init; }

    [JsonPropertyName("vote_average")] public double VoteAverage { get; init; }

    [JsonPropertyName("vote_count")] public int VoteCount { get; init; }

    [JsonPropertyName("popularity")] public double Popularity { get; init; }

    [JsonPropertyName("runtime")] public int Runtime { get; init; }

    [JsonPropertyName("revenue")] public int Revenue { get; init; }

    [JsonPropertyName("budget")] public int Budget { get; init; }

    [JsonPropertyName("genres")] public List<Genre> Genres { get; init; } = [];
    
    [JsonPropertyName("production_countries")] public List<ProductionCountry> ProductionCountries { get; init; } = [];

    [JsonPropertyName("original_language")]
    public string? OriginalLanguage { get; init; }
}

using System.Text.Json.Serialization;

namespace KinoBot.API.Models;

public class TvShow
{
    [JsonPropertyName("id")]
    public int Id { get; init; }
    
    [JsonPropertyName("name")]
    public required string Name { get; init; }
    
    [JsonPropertyName("overview")]
    public string? Overview { get; init; }
    
    [JsonPropertyName("first_air_date")]
    public string? FirstAirDate { get; init; }
    
    [JsonPropertyName("poster_path")]
    public string? PosterPath { get; init; }
    
    [JsonPropertyName("vote_average")]
    public double VoteAverage { get; init; }
    
    [JsonPropertyName("vote_count")]
    public int VoteCount { get; init; }
    
    [JsonPropertyName("popularity")]
    public double Popularity { get; init; }
    
    [JsonPropertyName("original_language")]
    public string? OriginalLanguage { get; init; }
}

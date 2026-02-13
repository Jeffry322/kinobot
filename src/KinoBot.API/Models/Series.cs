using System.Text.Json.Serialization;
using KinoBot.API.Abstractions;
using KinoBot.API.Views;

namespace KinoBot.API.Models;

public sealed class Series : IMedia
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

    [JsonPropertyName("next_episode_to_air")]
    public string? NextEpisodeToAir { get; set; }

    [JsonPropertyName("number_of_episodes")]
    public int NumberOfEpisodes { get; set; } = 0;

    [JsonPropertyName("number_of_seasons")]
    public int NumberOfSeasons { get; set; } = 0;

    [JsonPropertyName("original_language")]
    public string? OriginalLanguage { get; init; }

    [JsonPropertyName("original_name")]
    public string? OriginalName { get; init; }

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;
    
    [JsonPropertyName("tagline")]
    public string? Tagline { get; set; }
    
    [JsonPropertyName("created_by")]
    public List<CreatedBy> CreatedBys { get; set; } = [];

    [JsonPropertyName("genres")]
    public List<Genre> Genres { get; set; } = [];
    
    [JsonPropertyName("production_companies")]
    public List<ProductionCompany> ProductionCompanies { get; set; } = [];
    
    [JsonPropertyName("production_countries")]
    public List<ProductionCountry> ProductionCountries { get; set; } = [];
    
    [JsonPropertyName("last_episode_to_air")]
    public SeriesEpisode LastEpisode { get; set; } = new();

    [JsonPropertyName("external_ids")]
    public ExternalIds? ExternalIds { get; set; } = new();
    
    public IMediaView ToView()
    {
        return new SeriesDetailsView(this);
    }
}
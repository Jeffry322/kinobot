using System.Text.Json.Serialization;

namespace KinoBot.API.Models;

public sealed class SeriesEpisode
{
    [JsonPropertyName("id")]
    public int Id { get; init; } = 0;

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("overview")]
    public string? Overview { get; init; }

    [JsonPropertyName("vote_average")]
    public double VoteAverage { get; init; } = 0;

    [JsonPropertyName("vote_count")]
    public int VoteCount { get; init; } = 0;
    
    [JsonPropertyName("air_date")]
    public string? AirDate { get; init; }

    [JsonPropertyName("episode_number")]
    public int EpisodeNumber { get; init; } = 0;

    [JsonPropertyName("production_code")]
    public string? ProductionCode { get; init; }

    [JsonPropertyName("runtime")]
    public int Runtime { get; init; } = 0;

    [JsonPropertyName("season_number")]
    public int SeasonNumber { get; init; } = 0;

    [JsonPropertyName("show_id")]
    public int ShowId { get; init; } = 0;

    [JsonPropertyName("still_path")]
    public string? StillPath { get; init; }
}
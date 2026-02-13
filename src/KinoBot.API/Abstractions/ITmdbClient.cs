using KinoBot.API.Models;

namespace KinoBot.API.Abstractions;

public interface ITmdbClient
{
    Task<Movie?> GetMovieByIdAsync(int movieId, CancellationToken cancellationToken = default);
    Task<Series?> GetTvShowByIdAsync(int tvShowId, CancellationToken cancellationToken = default);
    Task<SearchResponse?> SearchMultiAsync(string query, int page, CancellationToken cancellationToken = default);
    Task<ExternalIds?> GetSeriesImdbId(int tvShowId, CancellationToken cancellationToken = default);
}

using KinoBot.API.Models;

namespace KinoBot.API.Abstractions;

public interface ITmdbClient
{
    Task<Movie?> GetMovieByIdAsync(int movieId, CancellationToken cancellationToken = default);
    Task<TvShow?> GetTvShowByIdAsync(int tvShowId, CancellationToken cancellationToken = default);
    Task<SearchResponse?> SearchMultiAsync(string query, int page, CancellationToken cancellationToken = default);
    Task<Stream> GetStreamAsync(string url, CancellationToken cancellationToken = default);
}

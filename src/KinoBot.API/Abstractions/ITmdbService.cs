using KinoBot.API.Models;

namespace KinoBot.API.Abstractions;

public interface ITmdbService 
{
    Task<Movie?> GetMovieByIdAsync(int movieId, CancellationToken cancellationToken = default);
    Task<TvShow?> GetTvShowByIdAsync(int tvShowId, CancellationToken cancellationToken = default);
    Task<SearchResponse?> SearchMultiAsync(string query, int page, CancellationToken cancellationToken = default);
}
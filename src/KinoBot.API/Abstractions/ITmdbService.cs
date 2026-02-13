using KinoBot.API.Models;

namespace KinoBot.API.Abstractions;

public interface ITmdbService 
{
    Task<IMedia?> GetMediaByIdAsync(int mediaId, string mediaType, CancellationToken cancellationToken = default);
    Task<Movie?> GetMovieByIdAsync(int movieId, CancellationToken cancellationToken = default);
    Task<Series?> GetTvShowByIdAsync(int tvShowId, CancellationToken cancellationToken = default);
    Task<SearchResponse?> SearchMultiAsync(string query, int page, CancellationToken cancellationToken = default);
}
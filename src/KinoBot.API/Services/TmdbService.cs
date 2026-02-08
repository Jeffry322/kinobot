using KinoBot.API.Abstractions;
using KinoBot.API.Models;
using Microsoft.Extensions.Caching.Hybrid;

namespace KinoBot.API.Services;

public sealed class TmdbService(HybridCache cache, ITmdbClient tmdbClient) : ITmdbService
{
    public async Task<Movie?> GetMovieByIdAsync(int movieId, CancellationToken cancellationToken = default)
    {
        var entry = await cache.GetOrCreateAsync(
            key: $"movie_{movieId}",
            factory: async (token) => await tmdbClient.GetMovieByIdAsync(movieId, token),
            cancellationToken: cancellationToken);
        
        return entry;
    }

    public async Task<TvShow?> GetTvShowByIdAsync(int tvShowId, CancellationToken cancellationToken = default)
    {
        var entry = await cache.GetOrCreateAsync(
            key: $"tvshow_{tvShowId}",
            factory: async (token) => await tmdbClient.GetTvShowByIdAsync(tvShowId, token),
            cancellationToken: cancellationToken);

        return entry;
    }

    public async Task<SearchResponse?> SearchMultiAsync(string query, int page, CancellationToken cancellationToken = default)
    {
        var entry = await cache.GetOrCreateAsync(
            key: $"search_{query}&page_{page}",
            options: new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromMinutes(10),
                LocalCacheExpiration = TimeSpan.FromMinutes(5)
            },
            factory: async token => await tmdbClient.SearchMultiAsync(query, page, token),
            cancellationToken: cancellationToken);
        
        return entry;
    }
}
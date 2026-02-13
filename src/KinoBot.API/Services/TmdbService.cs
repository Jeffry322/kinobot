using KinoBot.API.Abstractions;
using KinoBot.API.Models;
using Microsoft.Extensions.Caching.Hybrid;

namespace KinoBot.API.Services;

public sealed class TmdbService(HybridCache cache, ITmdbClient tmdbClient) : ITmdbService
{
    public async Task<IMedia?> GetMediaByIdAsync(int mediaId,
        string mediaType,
        CancellationToken cancellationToken = default)
    {
        return mediaType switch
        {
            "movie" => await GetMovieByIdAsync(mediaId, cancellationToken),
            "tv" => await GetTvShowByIdAsync(mediaId, cancellationToken),
            _ => throw new InvalidOperationException($"Media type '{mediaType}' is not supported.")
        };
    }

    public async Task<Movie?> GetMovieByIdAsync(int movieId, CancellationToken cancellationToken = default)
    {
        var entry = await cache.GetOrCreateAsync(
            key: $"movie_{movieId}",
            factory: async (token) => await tmdbClient.GetMovieByIdAsync(movieId, token),
            cancellationToken: cancellationToken);
        
        return entry;
    }

    public async Task<Series?> GetTvShowByIdAsync(int tvShowId, CancellationToken cancellationToken = default)
    {
        var factory = new Func<CancellationToken, Task<Series?>>(async token =>
        {
            var series = await tmdbClient.GetTvShowByIdAsync(tvShowId, token);
            if (series is null) return null;
            
            var externalIds = await tmdbClient.GetSeriesImdbId(tvShowId, token);
            series.ExternalIds = externalIds;

            return series;
        });
        
        var entry = await cache.GetOrCreateAsync(
            key: $"tvshow_{tvShowId}",
            factory: async (token) => await factory(token),
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
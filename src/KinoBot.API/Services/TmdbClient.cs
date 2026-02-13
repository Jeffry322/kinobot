using System.Text.Json;
using KinoBot.API.Abstractions;
using KinoBot.API.Models;

namespace KinoBot.API.Services;

public class TmdbClient : ITmdbClient
{
    private readonly HttpClient _httpClient;

    public TmdbClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }


    public async Task<Movie?> GetMovieByIdAsync(int movieId, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetAsync($"movie/{movieId}", cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var movie = JsonSerializer.Deserialize<Movie>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return movie;
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException($"Failed to fetch movie with ID {movieId} from TMDB API", ex);
        }
    }

    public async Task<Series?> GetTvShowByIdAsync(int tvShowId, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetAsync($"tv/{tvShowId}", cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonSerializer.Deserialize<Series>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException($"Failed to fetch TV show with ID {tvShowId} from TMDB API", ex);
        }
    }

    public async Task<SearchResponse?> SearchMultiAsync(string query, int page,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var encodedQuery = Uri.EscapeDataString(query);
            var response = await _httpClient
                .GetAsync($"search/multi?query={encodedQuery}&page={page}", cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var searchResponse = JsonSerializer.Deserialize<SearchResponse>(content);

            return searchResponse;
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException($"Failed to search for '{query}' from TMDB API", ex);
        }
    }

    public async Task<ExternalIds?> GetSeriesImdbId(int tvShowId, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetAsync($"tv/{tvShowId}/external_ids", cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonSerializer.Deserialize<ExternalIds>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException($"Failed to fetch external Ids for show {tvShowId} from TMDB API", ex);
        }
    }
}
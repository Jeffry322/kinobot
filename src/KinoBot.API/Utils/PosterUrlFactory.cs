namespace KinoBot.API.Utils;

public static class PosterUrlFactory
{
    public static string? GetPosterUrl(string? posterPath) =>
        posterPath is null ? null : $"https://image.tmdb.org/t/p/w500{posterPath}";
}
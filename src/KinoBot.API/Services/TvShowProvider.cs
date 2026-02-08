using System.Text;
using KinoBot.API.Abstractions;
using KinoBot.API.Models;

namespace KinoBot.API.Services;

public class TvShowProvider : IMediaProvider
{
    public string MediaType => "tv";

    public string FormatCaption(SearchResult result)
    {
        var year = result.DisplayDate.Split('-')[0];
        var title = result.DisplayName == result.DisplayOriginalName
            ? $"{result.DisplayName} ({year})"
            : $"{result.DisplayName} ({result.DisplayOriginalName}) ({year})";

        var builder = new StringBuilder();

        if (!string.IsNullOrEmpty(result.PosterPath))
        {
            var photoUrl = $"https://image.tmdb.org/t/p/w500{result.PosterPath}";
            builder.Append($"<a href=\"{photoUrl}\">&#8205;</a>");
        }

        builder.AppendLine($"<b>{title}</b>");
        builder.AppendLine($"⭐️ Rating: {result.VoteAverage:F1}/10");
        builder.AppendLine();
        builder.Append(result.Overview);

        return builder.ToString();
    }

    public string FormatCaption(TvShow tvShow)
    {
        var year = tvShow.FirstAirDate?.Split('-')[0] ?? "N/A";
        var title = $"{tvShow.Name} ({year})";

        var builder = new StringBuilder();

        if (!string.IsNullOrEmpty(tvShow.PosterPath))
        {
            var photoUrl = $"https://image.tmdb.org/t/p/w500{tvShow.PosterPath}";
            builder.Append($"<a href=\"{photoUrl}\">&#8205;</a>");
        }

        builder.AppendLine($"<b>{title}</b>");
        builder.AppendLine($"⭐️ Rating: {tvShow.VoteAverage:F1}/10 ({tvShow.VoteCount} votes)");
        builder.AppendLine($"🌐 Language: {tvShow.OriginalLanguage?.ToUpper()}");
        builder.AppendLine();
        builder.Append(tvShow.Overview);

        return builder.ToString();
    }

    public string FormatCaption(Movie movie) => throw new NotSupportedException();
}

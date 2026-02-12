using System.Text;
using KinoBot.API.Abstractions;
using KinoBot.API.Models;
using KinoBot.API.Utils;

namespace KinoBot.API.Services;

public class MovieProvider : IMediaProvider
{
    public string MediaType => "movie";

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

    public string FormatCaption(Movie movie)
    {
        var builder = new StringBuilder();
        
        var year = movie.ReleaseDate?.Split('-')[0] ?? "N/A";
        var title = $"{movie.Title} ({year})";
        var budget = movie.Budget == 0 ? "???" : MoneyFormatter.FormatMoney(movie.Budget);
        var revenue = movie.Revenue == 0 ? "???" : MoneyFormatter.FormatMoney(movie.Revenue);
        
        if (!string.IsNullOrEmpty(movie.PosterPath))
        {
            var photoUrl = $"https://image.tmdb.org/t/p/w500{movie.PosterPath}";
            builder.Append($"<a href=\"{photoUrl}\">&#8205;</a>");
        }

        builder.AppendLine($"<b>{title}</b>");
        builder.AppendLine();
        builder.AppendLine($"<blockquote>{movie.Tagline}</blockquote>");
        builder.AppendLine();
        builder.AppendLine($"⭐️ Rating: {movie.VoteAverage:F1}/10 ({movie.VoteCount} votes)");
        builder.AppendLine($"⏳ Runtime: {movie.Runtime} min");
        builder.AppendLine($"📅 Release Date: {movie.ReleaseDate}");
        builder.AppendLine($"💵 Budget: {budget}");
        builder.AppendLine($"💰 Revenue: {revenue}");
        
        if (movie.Genres.Count > 0)
        {
            builder.AppendLine($"🎭 Genres: {string.Join(", ", movie.Genres.Select(g => g.Name))}");
        }

        if (movie.ProductionCountries.Count > 0)
        {
            var singleOrPlural = movie.ProductionCountries.Count > 1 ? "🌎 Countries" : "🌎 Country";
            builder
                .Append(singleOrPlural)
                .Append(": ")
                .AppendJoin(", ", movie.ProductionCountries
                    .Select(c => $"{CodeToFlagConvertor.ToFlag(c.Code)} {c.Name}"));
            builder.AppendLine();
        }
        
        builder.AppendLine();
        builder.Append(movie.Overview);

        return builder.ToString();
    }

    public string FormatCaption(TvShow tvShow) => throw new NotSupportedException();
}

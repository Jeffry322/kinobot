using System.Text;
using KinoBot.API.Abstractions;
using KinoBot.API.CallbackData;
using KinoBot.API.Common.Factories;
using KinoBot.API.Common.Utils;
using KinoBot.API.Models;
using Telegram.Bot.Types.ReplyMarkups;

namespace KinoBot.API.Views;

public sealed class MovieDetailsView(
    Movie movie) : IMediaView
{
    public string GetFormatedCaption()
    {
        var builder = new StringBuilder();

        var year = movie.ReleaseDate?.Split('-')[0] ?? "N/A";
        var title = $"{movie.Title} ({year})";
        var budget = MoneyFormatter.FormatMoney(movie.Budget);
        var revenue = MoneyFormatter.FormatMoney(movie.Revenue);

        if (!string.IsNullOrEmpty(movie.PosterPath))
        {
            var photoUrl = $"https://image.tmdb.org/t/p/w500{movie.PosterPath}";
            builder.Append($"<a href=\"{photoUrl}\">&#8205;</a>");
        }

        builder.AppendLine($"<b>{title}</b>");

        if (!string.IsNullOrEmpty(movie.Tagline))
        {
            builder.AppendLine();
            builder.AppendLine($"<blockquote>{movie.Tagline}</blockquote>");
        }

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

    public InlineKeyboardMarkup GetReplyMarkup()
    {
        return new InlineKeyboardMarkup(
            InlineButtonFactory.WithCallbackData("👥 Actors",
                new GetActorsCallbackData { MediaId = movie.Id, MediaType = "movie" }),
            InlineButtonFactory.WithCallbackData("👁 Add to Watchlist",
                new AddMediaToWatchlistCallbackData { MediaId = movie.Id, MediaType = "movie" }),
            InlineKeyboardButton.WithUrl("🎬 IMDb", $"https://www.imdb.com/title/{movie.ImdbId}"));
    }
}
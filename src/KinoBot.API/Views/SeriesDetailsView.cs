using System.Text;
using KinoBot.API.Abstractions;
using KinoBot.API.Common.Utils;
using KinoBot.API.Models;
using Telegram.Bot.Types.ReplyMarkups;

namespace KinoBot.API.Views;

public sealed class SeriesDetailsView(Series series) : IMediaView
{
    public string GetFormatedCaption()
    {
        var builder = new StringBuilder();

        var year = series.FirstAirDate?.Split('-')[0] ?? "N/A";
        var title = $"{series.Name} ({year})";

        if (!string.IsNullOrEmpty(series.PosterPath))
        {
            var photoUrl = $"https://image.tmdb.org/t/p/w500{series.PosterPath}";
            builder.Append($"<a href=\"{photoUrl}\">&#8205;</a>");
        }

        builder.AppendLine($"<b>{title}</b>");

        if (!string.IsNullOrEmpty(series.Tagline))
        {
            builder.AppendLine();
            builder.AppendLine($"<blockquote>{series.Tagline}</blockquote>");
        }

        builder.AppendLine();
        builder.AppendLine($"⭐️ Rating: {series.VoteAverage:F1}/10 ({series.VoteCount} votes)");
        builder.AppendLine($"📅 First Air Date: {series.FirstAirDate}");
        builder.AppendLine($"📺 Seasons: {series.NumberOfSeasons}");
        builder.AppendLine($"🎞 Episodes: {series.NumberOfEpisodes}");
        builder.AppendLine($"🔄 Status: {series.Status}");

        if (series.Genres.Count > 0)
        {
            builder.AppendLine($"🎭 Genres: {string.Join(", ", series.Genres.Select(g => g.Name))}");
        }

        if (series.ProductionCountries.Count > 0)
        {
            var singleOrPlural = series.ProductionCountries.Count > 1 ? "🌎 Countries" : "🌎 Country";
            builder
                .Append(singleOrPlural)
                .Append(": ")
                .AppendJoin(", ", series.ProductionCountries
                    .Select(c => CodeToFlagConvertor.ToFlag(c.Code)));
            builder.AppendLine();
        }

        builder.AppendLine();
        builder.Append(series.Overview);

        return builder.ToString();
    }

    public InlineKeyboardMarkup GetReplyMarkup()
    {
        return new InlineKeyboardMarkup(
            InlineKeyboardButton.WithUrl("IMDb", $"https://www.imdb.com/title/{series.ExternalIds?.ImdbId}"));
    }
}
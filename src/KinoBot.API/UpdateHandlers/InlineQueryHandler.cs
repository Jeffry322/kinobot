using KinoBot.API.Abstractions;
using KinoBot.API.Common.Factories;
using KinoBot.API.Updates;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;

namespace KinoBot.API.UpdateHandlers;

public sealed class InlineQueryHandler(
    ITmdbService tmdbService) : IUpdateHandler<InlineQueryUpdate>
{
    public bool CanHandle(InlineQueryUpdate update)
    {
        return true;
    }

    public async Task HandleAsync(InlineQueryUpdate update,
        ITelegramBotClient bot,
        CancellationToken ct = default)
    {
        var inlineQuery = update.InlineQuery;

        var replyMarkup = new InlineKeyboardMarkup(
            InlineKeyboardButton.WithCallbackData("🎬", "null")
        );

        if (string.IsNullOrEmpty(inlineQuery.Query))
        {
            var defaultInlineResults = new List<InlineQueryResult>();

            var entry = new InlineQueryResultArticle(
                id: "random",
                title: "Random movie from watchlist",
                new InputTextMessageContent("Random movie from watchlist"))
            {
                ReplyMarkup = replyMarkup
            };

            defaultInlineResults.Add(entry);

            await bot.AnswerInlineQuery(inlineQuery.Id, defaultInlineResults, cancellationToken: ct);
            return;
        }

        int currentPage = 1;
        if (!string.IsNullOrEmpty(inlineQuery.Offset) && int.TryParse(inlineQuery.Offset, out int parsedPage))
        {
            currentPage = parsedPage;
        }

        var results = await tmdbService.SearchMultiAsync(inlineQuery.Query, currentPage, ct);
        if (results == null)
            return;

        var inlineResults = results.Results
            .Where(r => string.Equals(r.MediaType, "movie", StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(r.MediaType, "tv", StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(r => r.Popularity)
            .Select(result =>
            {
                var year = result.DisplayDate.Split('-')[0];

                var title = result.DisplayName == result.DisplayOriginalName
                    ? $"{result.DisplayName} ({year})"
                    : $"{result.DisplayName} ({result.DisplayOriginalName}) ({year})";

                var description = result.VoteAverage > 0
                    ? $"Rating: {result.VoteAverage:F1}/10 | {result.MediaTypeDisplay.ToUpper()}"
                    : result.MediaTypeDisplay.ToUpper();

                return new InlineQueryResultArticle(
                    id: $"{result.MediaType}:{result.Id}",
                    title: title,
                    inputMessageContent: new InputTextMessageContent(title)
                    {
                        ParseMode = ParseMode.Html
                    })
                {
                    Description = description,
                    ThumbnailUrl = PosterUrlFactory.GetPosterUrl(result.PosterPath),
                    ReplyMarkup = replyMarkup
                };
            });

        string? nextOffset = currentPage < results.TotalPages
            ? (currentPage + 1).ToString()
            : null;

        await bot.AnswerInlineQuery(inlineQuery.Id, inlineResults, cancellationToken: ct, nextOffset: nextOffset);
    }
}
using System.Text.Json;
using KinoBot.API.Abstractions;
using KinoBot.API.CallbackData;
using KinoBot.API.Utils;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;
using IUpdateHandler = KinoBot.API.Abstractions.IUpdateHandler;

namespace KinoBot.API.Services;

public sealed class UpdateHandler(
    ILogger<UpdateHandler> logger,
    ITmdbService tmdbService,
    ICallbackQueryHandler<GetMediaDetailsCallbackData> getMediaDetailsCallbackQueryHandler,
    ICallbackQueryHandler<GetBackCallbackData> getBackCallbackQueryHandler,
    MediaProviderFactory mediaProviderFactory) : IUpdateHandler
{
    public async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken ct)
    {
        switch (update.Type)
        {
            case Telegram.Bot.Types.Enums.UpdateType.InlineQuery:
                await HandleInlineQuery(bot, update.InlineQuery!, ct);
                break;
            case Telegram.Bot.Types.Enums.UpdateType.CallbackQuery:
                await HandleCallbackQuery(bot, update, ct);
                break;
        }
    }

    private async Task HandleCallbackQuery(ITelegramBotClient bot, Update update, CancellationToken ct)
    {
        var query = update.CallbackQuery!;
        if (query.Data == null) return;

        var data = JsonSerializer.Deserialize<ICallbackData>(query.Data);

        if (data is GetMediaDetailsCallbackData detailsData)
        {
            await getMediaDetailsCallbackQueryHandler
                .HandleCallbackQuery(bot, query, detailsData, ct);
        }
        else if (data is GetBackCallbackData backData)
        {
            await getBackCallbackQueryHandler
                .HandleCallbackQuery(bot, query, backData, ct);
        }
    }

    private async Task HandleInlineQuery(ITelegramBotClient bot, InlineQuery inlineQuery, CancellationToken ct)
    {
        if (string.IsNullOrEmpty(inlineQuery.Query))
            return;

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

                var description = $"Rating: {result.VoteAverage:F1}/10 | {result.MediaTypeDisplay.ToUpper()}";

                var provider = mediaProviderFactory.GetProvider(result.MediaType);
                var caption = provider.FormatCaption(result);

                List<InlineKeyboardButton> buttons =
                [
                    InlineButtonFactory.WithCallbackData("➕ Get Details!",
                        new GetMediaDetailsCallbackData()
                        {
                            MediaId = result.Id,
                            MediaType = result.MediaTypeDisplay,
                            SenderId = inlineQuery.From.Id
                        }),
                ];
                return new InlineQueryResultArticle(
                    id: $"{result.MediaType}:{result.Id}",
                    title: title,
                    inputMessageContent: new InputTextMessageContent(caption)
                    {
                        ParseMode = Telegram.Bot.Types.Enums.ParseMode.Html
                    })
                {
                    Description = description,
                    ThumbnailUrl = PosterUrlFactory.GetPosterUrl(result.PosterPath),
                    ReplyMarkup = buttons
                };
            });
        
        string? nextOffset = currentPage < results.TotalPages 
            ? (currentPage + 1).ToString() 
            : null;
        
        await bot.AnswerInlineQuery(inlineQuery.Id, inlineResults, cancellationToken: ct, nextOffset: nextOffset);
    }

    public Task HandleErrorAsync(ITelegramBotClient bot,
        Exception exception,
        HandleErrorSource source,
        CancellationToken ct)
    {
        logger.LogError(exception, "Error while handling update: {Message}", exception.Message);
        return Task.CompletedTask;
    }
}
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
            case UpdateType.InlineQuery:
                await HandleInlineQuery(bot, update.InlineQuery!, ct);
                break;
            case UpdateType.CallbackQuery:
                await HandleCallbackQuery(bot, update, ct);
                break;
            case UpdateType.ChosenInlineResult:
                await HandleChosenInlineResult(bot, update.ChosenInlineResult!, ct);
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

                var replyMarkup = new InlineKeyboardMarkup(
                    InlineKeyboardButton.WithCallbackData("🎬", "null")
                );
                
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

    private async Task HandleChosenInlineResult(ITelegramBotClient bot,
        ChosenInlineResult chosenInlineResult,
        CancellationToken ct)
    {
        var mediaType = chosenInlineResult.ResultId.Split(':')[0];
        var id = chosenInlineResult.ResultId.Split(':')[1];

        InlineMessage editMessage;
        
        if (string.Equals(mediaType, "movie", StringComparison.OrdinalIgnoreCase))
        {
            var movie = await tmdbService.GetMovieByIdAsync(int.Parse(id), ct);
            if (movie == null) return;
            
            var caption = mediaProviderFactory.GetProvider(mediaType).FormatCaption(movie);
            editMessage = new InlineMessage
            {
                CaptionHtml = caption,
                InlineMessageId = chosenInlineResult!.InlineMessageId!
            };
        }
        else
        {
            var tvShow = await tmdbService.GetTvShowByIdAsync(int.Parse(id), ct);
            if (tvShow is null) return;

            var caption = mediaProviderFactory.GetProvider(mediaType).FormatCaption(tvShow);
            editMessage = new InlineMessage
            {
                CaptionHtml = caption,
                InlineMessageId = chosenInlineResult!.InlineMessageId!
            };
        }

        await bot.EditMessageCaption(editMessage, ct);
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
using KinoBot.API.Abstractions;
using KinoBot.API.CallbackData;
using KinoBot.API.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace KinoBot.API.CallbackQueriesHandlers;

public sealed class GetMediaDetailsCallbackQuery(
    ILogger<GetMediaDetailsCallbackQuery> logger,
    ITmdbService tmdbService,
    MediaProviderFactory mediaProviderFactory)
    : ICallbackQueryHandler<GetMediaDetailsCallbackData>
{
    public async Task HandleCallbackQuery(ITelegramBotClient bot,
        CallbackQuery query,
        GetMediaDetailsCallbackData data,
        CancellationToken ct)
    {
        logger.LogInformation("Handling media details callback for {MediaId} ({MediaType})", data.MediaId,
            data.MediaType);

        if (query.From.Id != data.SenderId || query.InlineMessageId == null)
        {
            await bot.AnswerCallbackQuery(query.Id, "Nuh uh ☝️", cancellationToken: ct);
            return;
        }

        var provider = mediaProviderFactory.GetProvider(data.MediaType);

        string caption;
        InlineKeyboardMarkup replyMarkup;
        if (string.Equals(data.MediaType, "movie", StringComparison.OrdinalIgnoreCase))
        {
            var movie = await tmdbService.GetMovieByIdAsync(data.MediaId, ct);
            if (movie == null) return;
            caption = provider.FormatCaption(movie);

            replyMarkup = new InlineKeyboardMarkup(
                InlineKeyboardButton.WithUrl("IMDb", $"https://www.imdb.com/title/{movie.ImdbId}")
            );
        }
        else
        {
            var tv = await tmdbService.GetTvShowByIdAsync(data.MediaId, ct);
            if (tv == null) return;
            caption = provider.FormatCaption(tv);
            replyMarkup = new InlineKeyboardMarkup(
                InlineKeyboardButton.WithCallbackData("Go back", "null"));
        }

        await bot.EditMessageCaption(
            inlineMessageId: query.InlineMessageId,
            caption: caption,
            parseMode: Telegram.Bot.Types.Enums.ParseMode.Html,
            replyMarkup: replyMarkup,
            cancellationToken: ct);
    }
}
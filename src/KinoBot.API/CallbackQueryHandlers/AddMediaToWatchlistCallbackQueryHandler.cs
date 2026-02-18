using KinoBot.API.Abstractions;
using KinoBot.API.CallbackData;
using Kinobot.Domain.Entities;
using Kinobot.Shared;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace KinoBot.API.CallbackQueryHandlers;

public sealed class AddMediaToWatchlistCallbackQueryHandler(
    IWatchlistService watchlistService)
    : ICallbackQueryHandler<AddMediaToWatchlistCallbackData>
{
    public async Task HandleCallbackQuery(ITelegramBotClient bot,
        CallbackQuery query,
        AddMediaToWatchlistCallbackData data,
        CancellationToken ct = default)
    {
        var userId = query.From.Id;
        var result = await watchlistService.AddMediaToWatchlistAsync(data.MediaId, data.MediaType, userId, ct);

        if (!result.IsSuccess)
        {
            var message = result.Error?.Code switch
            {
                WatchlistMediaErrors.AlreadyInWatchlistCode => "Already in watchlist!",
                DatabaseErrors.SaveChangesErrorCode => "Something went wrong. Please try again later.",
                _ => "Could not add to watchlist."
            };

            await bot.AnswerCallbackQuery(query.Id,
                message,
                false,
                cancellationToken: ct);
            return;
        }
        
        await bot.AnswerCallbackQuery(query.Id,
            "Added to watchlist!",
            false,
            cancellationToken: ct);
    }
}
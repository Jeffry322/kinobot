using System.Text.Json;
using KinoBot.API.Abstractions;
using KinoBot.API.CallbackData;
using KinoBot.API.Updates;
using Kinobot.Domain.Entities;
using Kinobot.Shared;
using Telegram.Bot;

namespace KinoBot.API.UpdateHandlers;

public sealed class AddMediaToWatchlistCallbackQueryHandler(
    IWatchlistService watchlistService) 
    : IUpdateHandler<CallbackQueryUpdate>
{
    public bool CanHandle(CallbackQueryUpdate update)
    {
        return update.CallbackQuery.Data != null 
            && JsonSerializer.Deserialize<ICallbackData>(update.CallbackQuery.Data) is AddMediaToWatchlistCallbackData;
    }

    public async Task HandleAsync(CallbackQueryUpdate update,
        ITelegramBotClient bot,
        CancellationToken ct = default)
    {
        var userId = update.CallbackQuery.From.Id;
        var queryId = update.CallbackQuery.Id;
        var data = JsonSerializer.Deserialize<AddMediaToWatchlistCallbackData>(update.CallbackQuery.Data!)!;
        
        var result = await watchlistService.AddMediaToWatchlistAsync(data.MediaId, data.MediaType, userId, ct);

        if (!result.IsSuccess)
        {
            var message = result.Error?.Code switch
            {
                WatchlistMediaErrors.AlreadyInWatchlistCode => "Already in watchlist!",
                DatabaseErrors.SaveChangesErrorCode => "Something went wrong. Please try again later.",
                _ => "Could not add to watchlist."
            };
            
            await bot.AnswerCallbackQuery(queryId,
                message,
                false,
                cancellationToken: ct);
            return;
        }
        
        await bot.AnswerCallbackQuery(queryId,
            "Added to watchlist!",
            false,
            cancellationToken: ct);
    }
}
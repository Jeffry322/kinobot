using System.Text.Json;
using KinoBot.API.Abstractions;
using KinoBot.API.CallbackData;
using KinoBot.API.Updates;
using Telegram.Bot;

namespace KinoBot.API.UpdateHandlers;

public sealed class CallbackQueryHandler(
    ICallbackQueryHandler<AddMediaToWatchlistCallbackData> addToWatchlistQueryHandler) 
    : IUpdateHandler<CallbackQueryUpdate>
{
    public bool CanHandle(CallbackQueryUpdate update)
    {
        return true;
    }

    public async Task HandleAsync(CallbackQueryUpdate update, ITelegramBotClient bot, CancellationToken ct = default)
    {
        var callbackQuery = update.CallbackQuery;
        
        if (callbackQuery.Data == null) return;

        var data = JsonSerializer.Deserialize<ICallbackData>(callbackQuery.Data);

        if (data is AddMediaToWatchlistCallbackData addMediaToWatchlistCallbackData)
        {
            await addToWatchlistQueryHandler.HandleCallbackQuery(bot, callbackQuery, addMediaToWatchlistCallbackData,
                ct);
        }
    }
}
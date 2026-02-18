using KinoBot.API.Abstractions;
using KinoBot.API.CallbackData;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace KinoBot.API.CallbackQueryHandlers;

public class GetActorsCallbackQueryHandler(ITmdbService tmdbService) : ICallbackQueryHandler<GetActorsCallbackData>
{
    public async Task HandleCallbackQuery(ITelegramBotClient bot, CallbackQuery query, GetActorsCallbackData data,
        CancellationToken ct)
    {
        await tmdbService.GetActorsByIdAsync(data.MediaId, data.MediaType, ct);
    }
}
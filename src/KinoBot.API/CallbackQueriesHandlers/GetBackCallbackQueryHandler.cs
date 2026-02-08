using KinoBot.API.Abstractions;
using KinoBot.API.CallbackData;
using KinoBot.API.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace KinoBot.API.CallbackQueriesHandlers;

public sealed class GetBackCallbackQueryHandler(
    ILogger<GetBackCallbackQueryHandler> logger,
    ITmdbService tmdbService) : ICallbackQueryHandler<GetBackCallbackData>
{
    public async Task HandleCallbackQuery(ITelegramBotClient bot,
        CallbackQuery query,
        GetBackCallbackData data,
        CancellationToken ct)
    {
        
        await bot.AnswerCallbackQuery(query.Id, cancellationToken: ct);
    }
}
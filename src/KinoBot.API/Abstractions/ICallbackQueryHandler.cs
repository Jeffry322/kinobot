using Telegram.Bot;
using Telegram.Bot.Types;

namespace KinoBot.API.Abstractions;

public interface ICallbackQueryHandler<TCallbackData> where TCallbackData : ICallbackData
{
  Task HandleCallbackQuery(ITelegramBotClient bot,
    CallbackQuery query,
    TCallbackData data,
    CancellationToken ct);
}
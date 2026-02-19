using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace KinoBot.API.Abstractions;

public interface IUpdateService
{
    Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken ct);
    Task HandleErrorAsync(ITelegramBotClient bot, Exception exception, HandleErrorSource source, CancellationToken ct);
}
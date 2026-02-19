using Telegram.Bot;
using Telegram.Bot.Types;

namespace KinoBot.API.Abstractions;

public interface IUpdateDispatcher
{
    Task DispatchAsync(Update update, ITelegramBotClient bot, CancellationToken ct = default);
}
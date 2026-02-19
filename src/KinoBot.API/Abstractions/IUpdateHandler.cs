using Telegram.Bot;
using Telegram.Bot.Types;

namespace KinoBot.API.Abstractions;

public interface IUpdateHandler<in TUpdate> where TUpdate : IUpdate
{
    bool CanHandle(TUpdate update);
    Task HandleAsync(TUpdate update, ITelegramBotClient bot, CancellationToken ct = default);
}
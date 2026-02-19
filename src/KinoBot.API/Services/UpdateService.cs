using KinoBot.API.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace KinoBot.API.Services;

public sealed class UpdateService(
    ILogger<UpdateService> logger,
    IUpdateDispatcher dispatcher) : IUpdateService
{
    public async Task HandleUpdateAsync(ITelegramBotClient bot,
        Update update,
        CancellationToken ct = default)
    {
        await dispatcher.DispatchAsync(update, bot, CancellationToken.None);
    }

    public Task HandleErrorAsync(ITelegramBotClient bot,
        Exception exception,
        HandleErrorSource source,
        CancellationToken ct = default)
    {
        logger.LogError(exception, "Error while handling update: {Message}", exception.Message);
        return Task.CompletedTask;
    }
}
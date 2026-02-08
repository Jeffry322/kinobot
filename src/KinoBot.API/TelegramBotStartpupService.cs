using KinoBot.API.Configs;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace KinoBot.API;

public class TelegramBotSetupService : IHostedService
{
    private readonly ITelegramBotClient _bot;
    private readonly IOptions<BotConfiguration> _config;
    private readonly ILogger<TelegramBotSetupService> _logger;

    public TelegramBotSetupService(
        ITelegramBotClient bot,
        IOptions<BotConfiguration> config,
        ILogger<TelegramBotSetupService> logger)
    {
        _bot = bot;
        _config = config;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var webhookUrl = _config.Value.BotWebhookUrl.AbsoluteUri;
        
        try
        {
            await _bot.SetWebhook(
                webhookUrl,
                allowedUpdates: [],
                secretToken: _config.Value.SecretToken,
                cancellationToken: cancellationToken);
            
            _logger.LogInformation("Webhook set to {WebhookUrl}", webhookUrl);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to set webhook");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
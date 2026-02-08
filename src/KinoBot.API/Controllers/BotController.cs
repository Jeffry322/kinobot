using KinoBot.API.Abstractions;
using KinoBot.API.Configs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace KinoBot.API.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class BotController(IOptions<BotConfiguration> config) : ControllerBase
{
    /*[HttpGet("setWebhook")]
    public async Task<string> SetWebHook(
        [FromServices] ITelegramBotClient bot,
        CancellationToken ct)
    {
        var webhookUrl = config.Value.BotWebhookUrl.AbsoluteUri;

        await bot.SetWebhook(webhookUrl,
            allowedUpdates: [],
            secretToken: config.Value.SecretToken,
            cancellationToken: ct);

        return $"Webhook set to {webhookUrl}";
    }*/

    [HttpPost]
    public async Task<IActionResult> Post(
        [FromBody] Update update,
        [FromServices] ITelegramBotClient bot,
        [FromServices] IUpdateHandler handleUpdateService,
        CancellationToken ct)
    {
        if (Request.Headers["X-Telegram-Bot-Api-Secret-Token"] != config.Value.SecretToken)
            return Forbid();
        try
        {
            await handleUpdateService.HandleUpdateAsync(bot,
                update,
                ct);
        }
        catch (Exception exception)
        {
            await handleUpdateService.HandleErrorAsync(bot,
                exception,
                Telegram.Bot.Polling.HandleErrorSource.HandleUpdateError,
                ct);
        }

        return Ok();
    }
}
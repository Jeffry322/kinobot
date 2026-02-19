using KinoBot.API.Abstractions;
using KinoBot.API.Common.Extensions;
using KinoBot.API.Updates;
using Telegram.Bot;

namespace KinoBot.API.UpdateHandlers;

public sealed class ChosenInlineResultHandler(
    ITmdbService tmdbService)
    : IUpdateHandler<ChosenInlineResultUpdate>
{
    public bool CanHandle(ChosenInlineResultUpdate update)
    {
        try
        {
            var mediaType = update.ChosenInlineResult.ResultId.Split(':')[0];
            var id = update.ChosenInlineResult.ResultId.Split(':')[1];

            return mediaType is "movie" or "tv" && int.TryParse(id, out _);
        }
        catch
        {
            return false;
        }
    }

    public async Task HandleAsync(ChosenInlineResultUpdate update, ITelegramBotClient bot, CancellationToken ct = default)
    {
        var mediaType = update.ChosenInlineResult.ResultId.Split(':')[0];
        var id = update.ChosenInlineResult.ResultId.Split(':')[1];

        var media = await tmdbService.GetMediaByIdAsync(int.Parse(id), mediaType, CancellationToken.None);
        
        if (media is null)
        {
            return;
        }
        
        var view = media.ToView();
        var inlineMessageId = update.ChosenInlineResult.InlineMessageId!;
        
        await bot.EditMessageCaption(view, inlineMessageId, CancellationToken.None);
    }
}
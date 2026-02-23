using KinoBot.API.Abstractions;
using KinoBot.API.Common.Extensions;
using KinoBot.API.Updates;
using Telegram.Bot;

namespace KinoBot.API.UpdateHandlers;

public sealed class GetRandomMediaChosenInlineResultHandler(
    ILogger<GetRandomMediaChosenInlineResultHandler> logger,
    IWatchlistService watchlistService)
    : IUpdateHandler<ChosenInlineResultUpdate>
{
    public bool CanHandle(ChosenInlineResultUpdate update)
    {
        return update.ChosenInlineResult.ResultId == "random";
    }

    public async Task HandleAsync(ChosenInlineResultUpdate update,
        ITelegramBotClient bot,
        CancellationToken ct = default)
    {
        var chosenInlineResult = update.ChosenInlineResult;
        
        var randomMedia = await GetRandomMovieFromWatchlist(chosenInlineResult.From.Id, ct);
        if (randomMedia is null) return;
        
        await bot.EditMessageCaption(randomMedia.ToView(), chosenInlineResult.InlineMessageId!, ct);
    }
    
    private async Task<IMedia?> GetRandomMovieFromWatchlist(long telegramUserId, CancellationToken ct)
    {
        var result = await watchlistService.GetRandomMediaFromWatchlistAsync(telegramUserId, ct);

        if (!result.IsSuccess)
        {
            return null;
        }
        
        return result.Value;
    }
}
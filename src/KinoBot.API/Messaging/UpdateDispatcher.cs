using KinoBot.API.Abstractions;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Telegram.Bot;
using Telegram.Bot.Types;
using UpdateAdapter = KinoBot.API.Common.Utils.UpdateAdapter;

namespace KinoBot.API.Messaging;

public sealed class UpdateDispatcher(IServiceProvider sp) : IUpdateDispatcher
{
    public Task DispatchAsync(Update update, ITelegramBotClient bot, CancellationToken ct = default)
    {
        var adaptedUpdate = UpdateAdapter.From(update);
        return DispatchInternal((dynamic)adaptedUpdate, bot, ct);
    }
    
    private async Task DispatchInternal<TUpdate>(TUpdate update, ITelegramBotClient bot, CancellationToken ct = default)
        where TUpdate : IUpdate
    {
        using var scope = sp.CreateScope();
        var scopedServices = scope.ServiceProvider;
        
        var handlers = scopedServices.GetServices<IUpdateHandler<TUpdate>>();

        var matched = handlers
            .Where(h => h.CanHandle(update))
            .ToList();

        if (matched.Count == 0)
        {
            throw new InvalidOperationException($"No handler found for update of type {typeof(TUpdate)}");
        }
        
        await matched[0].HandleAsync(update, bot, ct);
    }
}
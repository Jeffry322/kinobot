using KinoBot.API.Abstractions;
using KinoBot.API.Updates;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace KinoBot.API.Common.Utils;

public static class UpdateAdapter
{
    public static IUpdate From(Update update) => update.Type switch
    {
        UpdateType.ChosenInlineResult when update.ChosenInlineResult is not null
            => new ChosenInlineResultUpdate(update, update.ChosenInlineResult),
        UpdateType.CallbackQuery when update.CallbackQuery is not null
            => new CallbackQueryUpdate(update, update.CallbackQuery),
        UpdateType.InlineQuery when update.InlineQuery is not null
            => new InlineQueryUpdate(update, update.InlineQuery),

        _ => throw new ArgumentException($"Unknown update type {update.Type}", nameof(update))
    };
}
using KinoBot.API.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace KinoBot.API.Updates;

public sealed record CallbackQueryUpdate(Update Raw, CallbackQuery CallbackQuery) : IUpdate
{
    public UpdateType UpdateType => UpdateType.CallbackQuery;
}
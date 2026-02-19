using KinoBot.API.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace KinoBot.API.Updates;

public sealed record ChosenInlineResultUpdate(Update Raw, ChosenInlineResult ChosenInlineResult) : IUpdate
{
    public UpdateType UpdateType => UpdateType.ChosenInlineResult;
}
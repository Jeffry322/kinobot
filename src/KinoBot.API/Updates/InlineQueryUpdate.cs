using KinoBot.API.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace KinoBot.API.Updates;

public sealed record InlineQueryUpdate(Update Raw, InlineQuery InlineQuery) : IUpdate
{
    public UpdateType UpdateType => UpdateType.InlineQuery;
}
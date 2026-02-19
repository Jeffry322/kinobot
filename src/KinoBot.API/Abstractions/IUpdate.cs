using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace KinoBot.API.Abstractions;

public interface IUpdate
{
    Update Raw { get; }
    UpdateType UpdateType { get; }
}
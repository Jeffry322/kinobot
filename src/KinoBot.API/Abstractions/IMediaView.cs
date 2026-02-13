using Telegram.Bot.Types.ReplyMarkups;

namespace KinoBot.API.Abstractions;

public interface IMediaView
{
    string GetFormatedCaption();
    InlineKeyboardMarkup GetReplyMarkup();
}
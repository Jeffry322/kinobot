using System.Text.Json;
using KinoBot.API.Abstractions;
using Telegram.Bot.Types.ReplyMarkups;

namespace KinoBot.API.Utils;

public static class InlineButtonFactory
{
    public static InlineKeyboardButton WithCallbackData<T>(string text, T data) where T : ICallbackData
    {
        return InlineKeyboardButton.WithCallbackData(text,
            JsonSerializer.Serialize<ICallbackData>(data));
    }
}
using System.Text.Json;
using KinoBot.API.Abstractions;
using Telegram.Bot.Types.ReplyMarkups;

namespace KinoBot.API.Common.Factories;

public static class InlineButtonFactory
{
    public static InlineKeyboardButton WithCallbackData<T>(string text, T data) where T : ICallbackData
    {
        return InlineKeyboardButton.WithCallbackData(text,
            JsonSerializer.Serialize<ICallbackData>(data));
    }
}
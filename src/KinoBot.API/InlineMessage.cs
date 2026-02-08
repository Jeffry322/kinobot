using Telegram.Bot.Types.ReplyMarkups;

namespace KinoBot.API;

public sealed class InlineMessage
{
  public string InlineMessageId { get; set; } = string.Empty;
  public string CaptionHtml { get; set; } = string.Empty;
  public InlineKeyboardMarkup ReplyMarkup { get; set; } = new();
}
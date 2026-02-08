using Telegram.Bot;

namespace KinoBot.API;

public static class TelegramBotClientExtensions
{
  public static async Task EditMessageCaption(this ITelegramBotClient bot,
    InlineMessage inlineMessage,
    CancellationToken ct)
  {
    await bot.EditMessageCaption(
      inlineMessageId: inlineMessage.InlineMessageId,
      caption: inlineMessage.CaptionHtml,
      parseMode: Telegram.Bot.Types.Enums.ParseMode.Html,
      replyMarkup: inlineMessage.ReplyMarkup,
      cancellationToken: ct);
  }
}
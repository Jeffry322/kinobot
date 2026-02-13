using KinoBot.API.Abstractions;
using Telegram.Bot;

namespace KinoBot.API.Common.Extensions;

public static class TelegramBotClientExtensions
{
    extension(ITelegramBotClient bot)
    {
        public async Task EditMessageCaption(IMediaView view,
            string inlineMessageId,
            CancellationToken ct)
        {
            var caption = view.GetFormatedCaption();
            var replyMarkup = view.GetReplyMarkup();

            await bot.EditMessageCaption(
                inlineMessageId: inlineMessageId,
                caption: caption,
                parseMode: Telegram.Bot.Types.Enums.ParseMode.Html,
                replyMarkup: replyMarkup,
                cancellationToken: ct);
        }
    }
}
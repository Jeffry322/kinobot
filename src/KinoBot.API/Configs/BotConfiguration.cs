namespace KinoBot.API.Configs;

public sealed class BotConfiguration
{
    public string BotToken { get; init; } = null!;
    public Uri BotWebhookUrl { get; init; } = null!;
    public string SecretToken { get; init; } = null!;
}
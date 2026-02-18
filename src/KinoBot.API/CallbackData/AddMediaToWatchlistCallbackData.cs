using System.Text.Json.Serialization;
using KinoBot.API.Abstractions;

namespace KinoBot.API.CallbackData;

public sealed class AddMediaToWatchlistCallbackData : ICallbackData
{
    [JsonPropertyName("i")]
    public required int MediaId { get; set; }

    [JsonPropertyName("m")]
    public required string MediaType { get; set; } = string.Empty;
}
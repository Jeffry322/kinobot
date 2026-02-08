using System.Text.Json.Serialization;
using KinoBot.API.Abstractions;
using KinoBot.API.Enums;

namespace KinoBot.API.CallbackData;

public sealed class GetMediaDetailsCallbackData : ICallbackData
{
    [JsonPropertyName("id")] public int MediaId { get; set; }

    [JsonPropertyName("mt")] public string MediaType { get; set; } = string.Empty;

    [JsonPropertyName("sid")] public long SenderId { get; set; }
}
using System.Text.Json.Serialization;
using KinoBot.API.Abstractions;
using KinoBot.API.Enums;

namespace KinoBot.API.CallbackData;

public sealed class GetBackCallbackData : ICallbackData
{
    [JsonPropertyName("mt")] public string MediaType { get; set; } = string.Empty;

    [JsonPropertyName("id")] public int MediaId { get; set; }
}
using System.Text.Json.Serialization;
using KinoBot.API.Abstractions;

public sealed class GetActorsCallbackData : ICallbackData
{
    [JsonPropertyName("i")] public required int MediaId;
    [JsonPropertyName("m")] public required string MediaType;
}
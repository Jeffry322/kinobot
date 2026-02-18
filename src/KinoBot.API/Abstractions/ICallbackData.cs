using System.Text.Json.Serialization;
using KinoBot.API.CallbackData;

namespace KinoBot.API.Abstractions;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "t")]
[JsonDerivedType(typeof(GetActorsCallbackData), "ga")]
[JsonDerivedType(typeof(AddMediaToWatchlistCallbackData), "wl")]
public interface ICallbackData;
using System.Text.Json.Serialization;
using KinoBot.API.CallbackData;

namespace KinoBot.API.Abstractions;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "t")]
[JsonDerivedType(typeof(GetMediaDetailsCallbackData), typeDiscriminator: "dets")]
[JsonDerivedType(typeof(GetBackCallbackData), typeDiscriminator: "gb")]
public interface ICallbackData;
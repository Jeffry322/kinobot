using System.Text.Json.Serialization;

namespace KinoBot.API.Abstractions;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "t")]
public interface ICallbackData;
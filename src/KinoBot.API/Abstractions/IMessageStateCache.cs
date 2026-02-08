namespace KinoBot.API.Abstractions;

public interface IMessageStateCache
{
    string Set(InlineMessage message);
    InlineMessage? Get(string stateId);
}

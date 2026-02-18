namespace Kinobot.Domain.Entities;

public sealed class WatchlistMedia : BaseEntity
{
    public required long TelegramUserId { get; init; }
    public required string MediaType { get; init; }
    public required int MediaId { get; init; }
    public bool IsWatched { get; private set; } = false;
    public DateTime CreatedAtUtc { get; init; } = DateTime.UtcNow;

    public void MarkAsWatched()
    {
        IsWatched = true;
    }
}
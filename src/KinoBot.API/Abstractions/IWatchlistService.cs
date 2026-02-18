using Kinobot.Shared;

namespace KinoBot.API.Abstractions;

public interface IWatchlistService
{
    Task<Result<IMedia?>> GetRandomMediaFromWatchlistAsync(long telegramUserId, CancellationToken cancellationToken = default);
    
    Task<Result> AddMediaToWatchlistAsync(int mediaId, string mediaType, long telegramUserId,
        CancellationToken cancellationToken = default);
}


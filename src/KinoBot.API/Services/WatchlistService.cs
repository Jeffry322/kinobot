using KinoBot.API.Abstractions;
using Kinobot.Domain.Entities;
using Kinobot.Infrastructure.Database;
using Kinobot.Shared;
using Microsoft.EntityFrameworkCore;

namespace KinoBot.API.Services;

public sealed class WatchlistService(
    ApplicationDbContext dbContext,
    ITmdbService tmdbService) : IWatchlistService
{
    public async Task<Result<IMedia?>> GetRandomMediaFromWatchlistAsync(long telegramUserId,
        CancellationToken cancellationToken = default)
    {
        var count = await dbContext.WatchlistMedias
            .Where(x => x.TelegramUserId == telegramUserId)
            .CountAsync(CancellationToken.None);

        if (count == 0)
        {
            return Result<IMedia?>.Failure(WatchlistMediaErrors.NotFound);
        }

        var skip = Random.Shared.Next(0, count);

        var row = await dbContext.WatchlistMedias
            .Where(x => x.TelegramUserId == telegramUserId)
            .OrderBy(x => x.Id)
            .Skip(skip)
            .Take(1)
            .AsNoTracking()
            .FirstOrDefaultAsync(CancellationToken.None);

        if (row is null)
        {
            return Result<IMedia?>.Failure(WatchlistMediaErrors.NotFound);
        }

        var media = await tmdbService.GetMediaByIdAsync(row.MediaId, row.MediaType, cancellationToken);

        return Result<IMedia?>.Success(media);
    }

    public async Task<Result> AddMediaToWatchlistAsync(int mediaId,
        string mediaType,
        long telegramUserId,
        CancellationToken cancellationToken = default)
    {
        var alreadyExists = await dbContext.WatchlistMedias
            .AsNoTracking()
            .AnyAsync(x => x.TelegramUserId == telegramUserId && x.MediaType == mediaType && x.MediaId == mediaId,
                cancellationToken);

        if (alreadyExists)
        {
            return Result.Failure(WatchlistMediaErrors.AlreadyInWatchlist);
        }

        var watchlistEntry = new WatchlistMedia
        {
            MediaId = mediaId,
            MediaType = mediaType,
            TelegramUserId = telegramUserId
        };

        try
        {
            await dbContext.AddAsync(watchlistEntry, cancellationToken);
            var saved = await dbContext.SaveChangesAsync(cancellationToken);
            return saved > 0 ? Result.Success() : Result.Failure(DatabaseErrors.SaveChangesError);
        }
        catch (DbUpdateException)
        {
            return Result.Failure(DatabaseErrors.SaveChangesError);
        }
    }
}
using Kinobot.Shared;

namespace Kinobot.Domain.Entities;

public sealed class WatchlistMediaErrors
{
    public const string AlreadyInWatchlistCode = "Watchlist.AlreadyExists";
    public const string NotFoundCode = "Watchlist.NotFound";
    
    public static Error AlreadyInWatchlist = new(AlreadyInWatchlistCode,
        "Media is already in watchlist");

    public static Error NotFound = new(NotFoundCode, "Media wasn't found");
}
using Kinobot.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kinobot.Infrastructure.Database.Configuration;

public sealed class WatchlistMediaConfiguration : IEntityTypeConfiguration<WatchlistMedia>
{
    public void Configure(EntityTypeBuilder<WatchlistMedia> builder)
    {
        builder.HasIndex(x => new { x.TelegramUserId, x.MediaType, x.MediaId })
            .IsUnique();
    }
}
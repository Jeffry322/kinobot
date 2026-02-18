using Kinobot.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kinobot.Infrastructure.Database;

public sealed class ApplicationDbContext : DbContext
{
    public DbSet<WatchlistMedia> WatchlistMedias { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
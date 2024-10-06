using MassTransit;
using Microsoft.EntityFrameworkCore;
using RatingService.Data.Entities;

namespace RatingService.Data;

public class RatingDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Rating> Ratings { get; set; }
    public DbSet<Provider> Providers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();

        modelBuilder.Entity<Provider>()
            .HasMany(o => o.Ratings)
            .WithOne(oi => oi.Provider)
            .HasForeignKey(oi => oi.ProviderId);
    }
}
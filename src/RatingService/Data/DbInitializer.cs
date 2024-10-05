using Microsoft.EntityFrameworkCore;
using RatingService.Data.Entities;

namespace RatingService.Data;

public abstract class DbInitializer
{
    public static void InitDb(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var ratingDbContext = scope.ServiceProvider.GetService<RatingDbContext>();
        
        MigrateDatabase(ratingDbContext);
        SeedData(ratingDbContext);
    }

    private static void MigrateDatabase(RatingDbContext context)
    {
        context.Database.Migrate();
    }

    private static void SeedData(RatingDbContext context)
    {
        if (context.Ratings.Any())
            return;

        var providers = new List<Provider>
        {
            new()
            {
                Id = 1,
                Name = "Provider 1",
                AverageRating = 3.4,
                TotalRating = 10.2,
                RatingCount = 3
            },
            new()
            {
                Id = 2,
                Name = "Provider 2",
                AverageRating = 4,
                TotalRating = 4,
                RatingCount = 1
            }
        };

        context.Providers.AddRange(providers);


        var ratings = new List<Rating>
        {
            new()
            {
                Point = 3.2,
                CustomerId = 1,
                ProviderId = 1
            },
            new()
            {
                Point = 2,
                CustomerId = 2,
                ProviderId = 1
            },
            new()
            {
                Point = 5,
                CustomerId = 3,
                ProviderId = 1
            },
            new()
            {
                Point = 4,
                CustomerId = 1,
                ProviderId = 2
            }
        };

        context.AddRange(ratings);
        
        context.SaveChanges();
    }
}
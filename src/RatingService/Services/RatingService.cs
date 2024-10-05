using RatingService.Data;
using RatingService.Data.Entities;
using RatingService.Models;

namespace RatingService.Services;

public class RatingService(RatingDbContext dbContext, ILogger<RatingService> logger) : IRatingService
{
    public async Task CreateRatingAsync(CreateRatingRequestModel createRatingRequest)
    {
        var rating = new Rating
        {
            Point = createRatingRequest.Point,
            CreatedAt = DateTime.UtcNow,
            ProviderId = createRatingRequest.ProviderId,
            CustomerId = createRatingRequest.CustomerId
        };

        dbContext.Ratings.Add(rating);

        //TODO produce event

        var result = await dbContext.SaveChangesAsync() > 0;

        if (!result)
            throw new Exception("Error when saving rating to db"); //TODO send service exception and log

        logger.LogInformation("Created rating with id: {ratingId}", rating.Id);
    }
}
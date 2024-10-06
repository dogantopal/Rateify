using System.Net;
using Contracts;
using MassTransit;
using RatingService.Data;
using RatingService.Data.Entities;
using RatingService.Errors;
using RatingService.Models;

namespace RatingService.Services;

public class RatingService(RatingDbContext dbContext, ILogger<RatingService> logger, IPublishEndpoint publishEndpoint) : IRatingService
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
        
        var ratingCreatedMessage = new RatingCreated
        {
            Point = rating.Point,
            ProviderId = rating.ProviderId
        };
        await publishEndpoint.Publish(ratingCreatedMessage);

        var result = await dbContext.SaveChangesAsync() > 0;

        if (!result)
        {
            //TODO add log.
            throw new ServiceException(HttpStatusCode.InternalServerError, $"Error when saving rating to db");
        }

        logger.LogInformation("Created rating with id: {ratingId}", rating.Id);
    }
}
using System.Net;
using RatingService.Data;
using RatingService.Data.Entities;
using RatingService.Errors;
using RatingService.Models;

namespace RatingService.Services;

public class ProviderService(RatingDbContext dbContext, ILogger<ProviderService> logger) : IProviderService
{
    public async Task<GetAverageRatingResponseModel> GetAverageRatingByIdAsync(long providerId)
    {
        var provider = await dbContext.Providers.FindAsync(providerId);

        if (provider is null)
        {
            logger.LogError("Provider not exist with given id:{providerId}", providerId);
            throw new ServiceException(HttpStatusCode.NotFound, $"Provider not exist with given id:{providerId}");
        }

        return new GetAverageRatingResponseModel(provider.Id, provider.Name, provider.AverageRating);
    }

    public async Task UpdateAverageRatingAsync(long providerId, double newRatingPoint)
    {
        var provider = await dbContext.Providers.FindAsync(providerId);

        if (provider is null)
        {
            logger.LogError("Provider not exist with given id:{providerId}", providerId);
            throw new ServiceException(HttpStatusCode.NotFound, $"Provider not exist with given id:{providerId}");
        }

        provider.RatingCount++;
        provider.TotalRating += newRatingPoint;
        provider.AverageRating = CalculateAverageRating(provider);

        await dbContext.SaveChangesAsync();
    }

    private static double CalculateAverageRating(Provider provider)
    {
        return provider.TotalRating / provider.RatingCount;
    }
}
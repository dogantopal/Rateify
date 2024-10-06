using RatingService.Data;
using RatingService.Data.Entities;
using RatingService.Models;

namespace RatingService.Services;

public class ProviderService(RatingDbContext dbContext) : IProviderService
{
    public async Task<GetAverageRatingResponseModel> GetAverageRatingByIdAsync(long providerId)
    {
        var provider = await dbContext.Providers.FindAsync(providerId);

        return provider == null
            ? throw new Exception($"Provider not exist with given id:{providerId}") //TODO: send service exception and create log
            : new GetAverageRatingResponseModel(provider.Id, provider.Name, provider.AverageRating);
    }

    public async Task UpdateAverageRatingAsync(long providerId, double newRatingPoint)
    {
        var provider = await dbContext.Providers.FindAsync(providerId);

        if (provider == null)
            throw new Exception($"Provider not exist with given id:{providerId}"); //TODO: send service exception and create log

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
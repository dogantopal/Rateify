using RatingService.Data;
using RatingService.Models;

namespace RatingService.Services;

public class ProviderService(RatingDbContext dbContext) : IProviderService
{
    public async Task<GetAverageRatingResponseModel> GetAverageRatingByIdAsync(long providerId)
    {
        var provider = await dbContext.Providers.FindAsync(providerId);

        return provider == null
            ? throw new Exception($"Provider not exist with given id:{providerId}")  //TODO: send service exception and create log
            : new GetAverageRatingResponseModel(provider.Id, provider.Name, provider.AverageRating);
    }
}
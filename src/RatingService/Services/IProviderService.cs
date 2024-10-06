using RatingService.Models;

namespace RatingService.Services;

public interface IProviderService
{
    Task<GetAverageRatingResponseModel> GetAverageRatingByIdAsync(long providerId);
    Task UpdateAverageRatingAsync(long providerId, double newRatingPoint);
}
using RatingService.Models;

namespace RatingService.Services;

public interface IProviderService
{
    Task<GetAverageRatingResponseModel> GetAverageRatingByIdAsync(long providerId);
}
using RatingService.Models;

namespace RatingService.Services;

public interface IRatingService
{
    Task CreateRatingAsync(CreateRatingRequestModel createRatingRequest);
}
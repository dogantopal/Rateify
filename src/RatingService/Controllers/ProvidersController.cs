using Microsoft.AspNetCore.Mvc;
using RatingService.Models;
using RatingService.Services;

namespace RatingService.Controllers;

[Route("api/providers")]
[ApiController]
public class ProvidersController(IProviderService providerService) : ControllerBase
{
    [HttpGet("{id}/average-rating")]
    public async Task<ActionResult<GetAverageRatingResponseModel>> GetAverageRating(
        [FromRoute] GetAverageRatingRequestModel request)
    {
        var averageRatingResponseModel = await providerService.GetAverageRatingByIdAsync(request.Id);

        if (averageRatingResponseModel is null) return NotFound();

        return averageRatingResponseModel;
    }
}
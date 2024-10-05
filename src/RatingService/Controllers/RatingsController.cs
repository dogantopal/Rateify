using Microsoft.AspNetCore.Mvc;
using RatingService.Models;
using RatingService.Services;

namespace RatingService.Controllers;

[Route("api/ratings")]
[ApiController]
public class RatingsController(IRatingService ratingService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> CreateRating(CreateRatingRequestModel requestModel)
    {
        await ratingService.CreateRatingAsync(requestModel);

        return Created();
    }
}
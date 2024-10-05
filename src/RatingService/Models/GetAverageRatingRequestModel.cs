using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace RatingService.Models;

public class GetAverageRatingRequestModel
{
    [Range(1, long.MaxValue, ErrorMessage = "Please enter a value bigger than 0.")]
    [FromRoute(Name = "id")]
    public long Id { get; set; }
}

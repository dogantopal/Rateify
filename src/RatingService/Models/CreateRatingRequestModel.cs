using System.ComponentModel.DataAnnotations;

namespace RatingService.Models;

public record CreateRatingRequestModel
{
    [Range(1, 5, ErrorMessage = "Please enter a value between 1 and 5.")]
    public double Point { get; set; }

    [Range(1, long.MaxValue, ErrorMessage = "Please enter a value bigger than 0.")]
    public long ProviderId { get; set; }

    [Range(1, long.MaxValue, ErrorMessage = "Please enter a value bigger than 0.")]
    public long CustomerId { get; set; } //Normally this property handled by token etc. This is only for demo purpose.
}
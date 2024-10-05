namespace RatingService.Data.Entities;

public class Provider
{
    public long Id { get; set; }
    public string Name { get; set; }
    public double AverageRating { get; set; }
    public double TotalRating { get; set; }
    public long RatingCount { get; set; }
    
    public ICollection<Rating> Ratings { get; set; }
}
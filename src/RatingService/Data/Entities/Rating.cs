namespace RatingService.Data.Entities;

public class Rating
{
    public long Id { get; set; }
    public double Point { get; set; }
    public long CustomerId { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public long ProviderId { get; set; }
    public Provider Provider { get; set; }
}
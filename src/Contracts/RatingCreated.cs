namespace Contracts;

public record RatingCreated
{
    public double Point { get; set; }
    public long ProviderId { get; set; }
}
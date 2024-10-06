using Contracts;
using MassTransit;
using RatingService.Services;

namespace RatingService.Consumers;

public class RatingCreatedConsumer(IProviderService providerService, ILogger<RatingCreatedConsumer> logger) : IConsumer<RatingCreated>
{
    public async Task Consume(ConsumeContext<RatingCreated> context)
    {
        logger.LogInformation("--> Consuming rating created from rating service.");

        var serviceProviderId = context.Message.ProviderId;

        await providerService.UpdateAverageRatingAsync(serviceProviderId, context.Message.Point);
    }
}

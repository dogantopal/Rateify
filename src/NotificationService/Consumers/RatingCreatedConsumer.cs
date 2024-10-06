using Contracts;
using MassTransit;
using NotificationService.Infrastructure;

namespace NotificationService.Consumers;

public class RatingCreatedConsumer(IConfiguration config, IRedisService redisService, ILogger<RatingCreatedConsumer> logger) : IConsumer<RatingCreated>
{
    public async Task Consume(ConsumeContext<RatingCreated> context)
    {
        logger.LogInformation("--> Consuming rating created from notification service.");

        var redisKey = config.GetValue<string>("NotificationStackRedisKey");

        var data = await redisService.GetValueAsync<List<RatingCreated>>(redisKey);

        if (data is null)
            data = [context.Message];
        else
            data.Add(context.Message);

        _ = await redisService.UpdateValueAsync(redisKey, data);
    }
}
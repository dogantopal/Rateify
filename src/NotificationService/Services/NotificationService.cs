using Contracts;
using NotificationService.Infrastructure;
using NotificationService.Models;

namespace NotificationService.Services;

public class NotificationService(IConfiguration config, IRedisService redisService)
    : INotificationService
{
    public async Task<List<Notification>> GetNotificationsAsync()
    {
        var redisKey = config.GetValue<string>("NotificationStackRedisKey");

        var data = await redisService.GetValueAsync<List<RatingCreated>>(redisKey);

        if (data is null)
            return new List<Notification>();

        await redisService.DeleteAsync(redisKey);

        return data.Select(x => new Notification(x.Point, x.ProviderId)).ToList();
    }
}
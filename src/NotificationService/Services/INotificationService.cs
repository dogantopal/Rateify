using NotificationService.Models;

namespace NotificationService.Services;

public interface INotificationService
{
    Task<List<Notification>> GetNotificationsAsync();
}
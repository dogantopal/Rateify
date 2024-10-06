using Microsoft.AspNetCore.Mvc;
using NotificationService.Models;
using NotificationService.Services;

namespace NotificationService.Controllers;

[Route("api/notifications")]
[ApiController]
public class NotificationsController(INotificationService notificationService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<GetNotificationsResponseModel>> GetAverageRating()
        => new GetNotificationsResponseModel(await notificationService.GetNotificationsAsync());
}
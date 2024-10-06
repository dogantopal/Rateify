using System.Net;

namespace NotificationService.Errors;

public class ServiceException(HttpStatusCode code, string errorMessage) : Exception
{
    public HttpStatusCode Code { get; set; } = code;
    public string ErrorMessage { get; set; } = errorMessage;
}
using System.Net;
using System.Text.Json;
using RatingService.Errors;

namespace RatingService.Middlewares;

public class ExceptionMiddleware(
    RequestDelegate next,
    ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, logger);
        }
    }
    private async Task HandleExceptionAsync(HttpContext context, Exception ex, ILogger<ExceptionMiddleware> logger)
    {
        object error = null;
        switch (ex)
        {
            case ServiceException se:
                logger.LogError(ex, "SERVICE ERROR");
                error = se.ErrorMessage;
                context.Response.StatusCode = (int)se.Code;
                break;
            case Exception e:
                logger.LogError(e, "SERVER ERROR");
                error = string.IsNullOrWhiteSpace(e.Message) ? "Error" : e.Message;
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;
        }
        context.Response.ContentType = "application/json";
        if (error != null)
        {
            var result = JsonSerializer.Serialize(new
            {
                error
            });
            await context.Response.WriteAsync(result);
        }
    }
}
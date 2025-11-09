using Bl.Financial.Size.Sample.Application.ValueObject;
using System.Net;
using System.Text.Json;

namespace Bl.Financial.Size.Sample.Server.Endpoints;

internal class CoreExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<CoreExceptionMiddleware> _logger;

    public CoreExceptionMiddleware(
        RequestDelegate next,
        ILogger<CoreExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (CoreException coreException)
        {
            _logger.LogInformation(coreException, "Expected exception in request {Path}", context.Request.Path);
            await HandleExceptionAsync(context, coreException);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception in request {Path}", context.Request.Path);
            throw;
        }
    }

    private Task HandleExceptionAsync(HttpContext context, CoreException exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        string result = JsonSerializer.Serialize(
            new object[]
            {
                new { ErrorMessage = exception.Message }
            }.ToArray());

        try
        {
            var statusCode =
                int.Parse(
                    string.Concat((exception.Code).ToString().Take(3))
                );

            response.StatusCode = (int)(HttpStatusCode)statusCode;
        }
        catch { }

        return response.WriteAsync(result);
    }
}

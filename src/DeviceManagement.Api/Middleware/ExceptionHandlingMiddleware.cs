using System.Net;
using System.Text.Json;
using DeviceManagement.Exception.ExceptionsBase;

namespace DeviceManagement.Api.Middleware;

/// <summary>
/// Middleware for global exception handling
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, System.Exception exception)
    {
        context.Response.ContentType = "application/json";

        // ✅ CORRIGIDO - Objeto anônimo padrão
        object response = new
        {
            message = exception.Message,
            details = exception.InnerException?.Message,
            timestamp = DateTime.UtcNow
        };

        switch (exception)
        {
            case ValidationErrorsException validationEx:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                // ✅ CORRIGIDO - Objeto anônimo com estrutura consistente
                response = new
                {
                    message = "Validation error",
                    details = (string?)null, // Mantém a estrutura consistente
                    timestamp = DateTime.UtcNow,
                    errors = validationEx.ErrorMessages
                };
                break;

            case NotFoundException:
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                break;

            case ConflictException:
                context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                break;

            default:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                // ✅ CORRIGIDO - Objeto anônimo com estrutura consistente
                response = new
                {
                    message = "Internal server error",
                    details = (string?)null,
                    timestamp = DateTime.UtcNow
                };
                break;
        }

        var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(jsonResponse);
    }
}

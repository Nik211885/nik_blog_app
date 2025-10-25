using System.Text.Json;
using Application.Exceptions;

namespace WebApi.Pipelines.Middlewares;

public class ExceptionHandlingMiddleware 
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly RequestDelegate _next;
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };
    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger, RequestDelegate next)
    {
        _logger = logger;
        _next = next;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }
    /// <summary>
    ///  Handle when action  has throw exception
    /// </summary>
    /// <param name="context">context for request</param>
    /// <param name="ex">exception action has throw</param>
    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        _logger.LogError(ex, ex.Message);
        if (context.Response.HasStarted)
        {
            return;
        }
        var statusCode = GetStatusCode(ex);
        var response = CreateResponse(ex, statusCode);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsync(JsonSerializer.Serialize(response, _jsonOptions));
    }
    /// <summary>
    ///     Get status for exception
    /// </summary>
    /// <param name="exception">compare match with  exception has custom</param>
    /// <returns>
    ///     Return status code fit with exception
    /// </returns>
    private static int GetStatusCode(Exception exception) => exception switch
    {
        BusinessException => StatusCodes.Status400BadRequest,
        NotFoundException => StatusCodes.Status404NotFound,
        UnauthorizedException => StatusCodes.Status401Unauthorized,
        NotImplementedException => StatusCodes.Status501NotImplemented,
        TimeoutException => StatusCodes.Status408RequestTimeout,
        _ => StatusCodes.Status500InternalServerError
    };
    /// <summary>
    ///     Create response body when application throw exception
    /// </summary>
    /// <param name="ex">exception application throw</param>
    /// <param name="statusCode">status code for exception</param>
    /// <returns>
    ///     Return object is containing data for response with exception
    /// </returns>
    private static object CreateResponse(Exception ex, int statusCode)
    {
        var title = ex.GetType().FullName;
        var traceId = System.Diagnostics.Activity.Current?.Id ?? Guid.NewGuid().ToString();
        string message = "Có lỗi xãy ra trong quá trình xử lý";
        if (statusCode < 500)
        {
            message = ex.Message;
        }
        return new
        {
            title =  title,
            traceId = traceId,
            message = message,
        };
    }
    
}


public static class ExceptionHandlingMiddlewareExtension
{
    /// <summary>
    ///  Add middle ware exception handing to pipeline
    /// </summary>
    /// <param name="app">application</param>
    /// <returns></returns>
    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}

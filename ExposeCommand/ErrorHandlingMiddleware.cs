using System.Net;
using System.Text.Json;

namespace ExposeCommand;

public class ErrorHandlingMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (BadHttpRequestException ex)
        {
            // TODO: Manage exception here
        }        
        catch (Exception ex)
        {
            // TODO: Manage interal error here
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.InternalServerError; // 500 if unexpected

        if (exception is JsonException) code = HttpStatusCode.BadRequest;

        var result = JsonSerializer.Serialize(new { error = "Internal Error" });
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;
        return context.Response.WriteAsync(result);
    }
}
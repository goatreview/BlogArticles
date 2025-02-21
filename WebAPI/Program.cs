var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Logging.AddOpenTelemetry(options =>
{
    options.IncludeFormattedMessage = true;
    options.IncludeScopes = true;
});

var app = builder.Build();

app.MapGet("/interpolated", (ILogger<Program> logger) => 
{
    var dateTime = DateTime.UtcNow;
    logger.LogInformation($"Interpolated : {dateTime}");
    return dateTime;
});
app.MapGet("/structured", (ILogger<Program> logger) => 
{
    var dateTime = DateTime.UtcNow;
    logger.LogInformation("Structured : {dateTime}", dateTime);
    return dateTime;
});
app.MapGet("/highperformance", (ILogger<Program> logger) => 
{
    var dateTime = DateTime.UtcNow;
    logger.LogDate(dateTime);
    return dateTime;
});

app.Run();

internal static partial class LogEntensions
{
    [LoggerMessage(EventId = 1000, Level = LogLevel.Information, Message = "High performance : {dateTime}")]
    public static partial void LogDate(this ILogger logger, DateTime dateTime);
}

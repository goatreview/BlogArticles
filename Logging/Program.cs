// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using Microsoft.Extensions.Logging;

BenchmarkRunner.Run<LoggerBenchmark>();

[SimpleJob(RuntimeMoniker.Net90)]
[MemoryDiagnoser]
public class LoggerBenchmark
{
    public LoggerBenchmark()
    {
        var factory = new LoggerFactory();
        _logger = factory.CreateLogger<LoggerBenchmark>();
    }
    
    private readonly ILogger _logger;
    private readonly DateTime _loginTime = DateTime.UtcNow;

    [Benchmark]
    public void InterpolatedLogging()
    {
        for (int i = 0; i < 1000; i++)
            _logger.LogInformation($"Goat number {i} logged in at {_loginTime}");
    }

    [Benchmark]
    public void StructuredLogging()
    {
        for (int i = 0; i < 1000; i++)
            _logger.LogInformation("Goat number {i} logged in at {LoginTime}", i, _loginTime);
    }

    [Benchmark]
    public void HighPerformanceLogging()
    {
        for (int i = 0; i < 1000; i++)
            _logger.UserLoggedIn(i, _loginTime);
    }
}

internal static partial class HighPerformanceLoggingExtension
{
    [LoggerMessage(EventId = 1000, Level = LogLevel.Information, Message = "Goat number {Id} logged in at {LoginTime}")]
    public static partial void UserLoggedIn(this ILogger logger, int id, DateTime loginTime);
}
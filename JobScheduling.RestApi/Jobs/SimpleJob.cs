using Quartz;
using Serilog;
using ILogger = Serilog.ILogger;

namespace JobScheduling.RestApi.Jobs;

internal sealed class SimpleJob : IJob
{
    private readonly ILogger _log = Log.ForContext<SimpleJob>();

    public static readonly JobKey Key = new("simple");
    
    public Task Execute(IJobExecutionContext context)
    {
        _log.Information("Hello! Simple job here.");
        return Task.CompletedTask;
    }
}
using Quartz;

namespace JobScheduling.GettingStarted.Jobs;

public class LogJob : IJob
{
    public Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine($"Job {context.JobDetail.Key} says hello!");
        return Task.CompletedTask;
    }
}
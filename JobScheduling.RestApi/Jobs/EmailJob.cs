using JobScheduling.RestApi.Services;
using Quartz;

namespace JobScheduling.RestApi.Jobs;

internal sealed class EmailJob(IEmailService emailService) : IJob
{
    public static JobKey GetKey(string username)
    {
        return new JobKey(name: $"send-email-to-{username}", group: "email");
    } 
    
    public async Task Execute(IJobExecutionContext context)
    {
        if (!context.JobDetail.JobDataMap.TryGetString("username", out var username))
        {
            throw new InvalidOperationException("Email job requires a username!");
        }
        
        await emailService.SendEmailAsync($"Hello {username}! Email from {context.JobDetail.Key}");
    }
}
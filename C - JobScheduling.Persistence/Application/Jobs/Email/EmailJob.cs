using JobScheduling.Persistence.Application.Email;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace JobScheduling.Persistence.Application.Jobs.Email;

/// <summary>
/// Example job of sending an informational e-mail to a user.
/// </summary>
internal sealed class EmailJob(
    ApplicationDbContext dbContext, 
    IEmailService email,
    ILogger<EmailJob> logger) : IJob
{
    public static JobKey GetKey(Guid userId) => JobKey.Create(name: $"simple-email-{userId}", group: "email");
    
    public async Task Execute(IJobExecutionContext context)
    {
        logger.LogInformation("Starting {jobName}", nameof(EmailJob));

        var ct = context.CancellationToken;
        var userId = context.MergedJobDataMap.GetGuidValue("user-id");

        if (await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId, ct) is not { } user)
        {
            logger.LogError("User with ID '{userId}' does not exist. Cancelling job.", userId);
            return;
        }

        await email.SendAsync(
            to: user,
            subject: $"Hello, {user.Name}! It is {DateTime.UtcNow:f}",
            body: "How are you today?",
            token: ct);
        
        logger.LogInformation("Finished {jobName}", nameof(EmailJob));
    }
}
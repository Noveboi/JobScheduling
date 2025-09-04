using JobScheduling.Persistence.Application;
using JobScheduling.Persistence.Application.Jobs.Email;
using JobScheduling.Persistence.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace JobScheduling.Persistence.Endpoints;

internal static class JobEndpoints
{
    public static void MapJobEndpoints(this IEndpointRouteBuilder app)
    {
        var userGroup = app.GetUsersGroup();
        userGroup.MapPost("{userId:guid}/jobs", AddJob);
    }

    private sealed record AddJobRequest(
        string JobType, 
        int IntervalInSeconds);
    private static async Task<IResult> AddJob(
        [FromBody] AddJobRequest req,
        [FromRoute] Guid userId,
        [FromServices] ApplicationDbContext context,
        [FromServices] ISchedulerFactory schedulerFactory,
        CancellationToken ct)
    {
        if (await context.Users.FirstOrDefaultAsync(x => x.Id == userId, ct) is not { } user)
        {
            return Results.BadRequest("User does not exist");
        }

        if (!req.JobType.Equals("email", StringComparison.OrdinalIgnoreCase))
        {
            return Results.BadRequest($"Invalid job type `{req.JobType}`");
        }

        var jobDataMap = new JobDataMap
        {
            ["user-id"] = userId.ToString()
        };
        
        var job = JobBuilder.Create<EmailJob>()
            .WithIdentity(EmailJob.GetKey(user.Id))
            .SetJobData(jobDataMap)
            .WithDescription($"This job sends an email to the user {user.Name} every {req.IntervalInSeconds} seconds")
            .Build();

        var trigger = TriggerBuilder.Create()
            .WithIdentity($"email-trigger-{userId}")
            .ForJob(job)
            .StartNow()
            .WithSimpleSchedule(x => x.WithIntervalInSeconds(req.IntervalInSeconds).RepeatForever())
            .Build();

        var userJob = new UserJob
        {
            JobKey = job.Key.ToString(),
            Description = job.Description,
            UserId = user.Id
        };
        
        user.Jobs.Add(userJob);

        await context.SaveChangesAsync(ct);

        var scheduler = await schedulerFactory.GetScheduler(ct);

        await scheduler.ScheduleJob(job, trigger, ct);
        return Results.Ok();
    }
}
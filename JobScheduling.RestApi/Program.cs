using JobScheduling.RestApi.Jobs;
using JobScheduling.RestApi.Services;
using Microsoft.AspNetCore.Mvc;
using Quartz;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSerilog();
builder.Services.AddTransient<EmailJob>();
builder.Services.AddSingleton<IEmailService, EmailService>();

// This instantiates the scheduler and starts it from an ASP.NET Core Hosted Service. Very important!
builder.Services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

// This does the rest of the configuration! You can configure stuff like database connections here, we'll dive deeper
// in the next project!
builder.Services.AddQuartz(options =>
{
    // We create the SimpleJob at startup
    options.ScheduleJob<SimpleJob>(
        trigger: x => x
            .StartNow()
            .WithSimpleSchedule(s => s.WithIntervalInSeconds(5).RepeatForever()),
        job: x => x
            .WithIdentity(SimpleJob.Key));
});

var app = builder.Build();


/*
 * This endpoint creates an EmailJob that is parameterized by the HTTP request `CreateEmailJobRequest`.
 * The user can customize the job creation by specifying a username (to be passed to the job's data) and an interval.
 *
 * This demonstrates how we can give users the power to customize jobs. Of course, in a real world scenario we'd need to
 * be very careful with parameterization since we cannot inherently trust the user's inputs. For example, the user could
 * define a job with an interval of 0.00001 seconds and give us an Azure bill of €100,000!!!!!
 */
app.MapPost("email-job", async (CreateEmailJobRequest req, [FromServices] ISchedulerFactory factory) =>
{
    var dataMap = new JobDataMap
    {
        ["username"] = req.Username
    };

    var jobKey = EmailJob.GetKey(username: req.Username);
    
    var job = JobBuilder.Create<EmailJob>()
        .WithIdentity(jobKey)
        .SetJobData(dataMap)
        .WithDescription($"This job sends an email to the user {req.Username} every {req.SecondsInterval} seconds")
        .Build();

    var trigger = TriggerBuilder.Create()
        .WithIdentity($"email-trigger-{req.Username}")
        .ForJob(jobKey)
        .StartNow()
        .WithSimpleSchedule(x => x.WithIntervalInSeconds(req.SecondsInterval).RepeatForever())
        .Build();

    var scheduler = await factory.GetScheduler();
    
    await scheduler.ScheduleJob(job, trigger);
});


app.Run();
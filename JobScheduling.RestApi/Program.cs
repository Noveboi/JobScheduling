using JobScheduling.RestApi.Jobs;
using JobScheduling.RestApi.Services;
using Microsoft.AspNetCore.Mvc;
using Quartz;
using Quartz.Spi;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSerilog();
builder.Services.AddTransient<EmailJob>();
builder.Services.AddSingleton<IEmailService, EmailService>();

builder.Services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);
builder.Services.AddQuartz(options =>
{
    options.ScheduleJob<SimpleJob>(
        trigger: x => x
            .StartNow()
            .WithSimpleSchedule(s => s.WithIntervalInSeconds(5).RepeatForever()),
        job: x => x
            .WithIdentity(SimpleJob.Key));
});

var app = builder.Build();

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
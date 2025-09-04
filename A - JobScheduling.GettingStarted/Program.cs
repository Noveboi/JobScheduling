using JobScheduling.GettingStarted.Jobs;
using Quartz;
using Quartz.Impl;

var schedulerFactory = new StdSchedulerFactory();
var scheduler = await schedulerFactory.GetScheduler();

await scheduler.Start();

var job = JobBuilder.Create<LogJob>()
    .WithIdentity("log-job", "main-group")
    .Build();

var trigger = TriggerBuilder.Create()
    .WithIdentity("every-second", "main-group")
    .StartNow()
    .WithSimpleSchedule(x => x
        .WithIntervalInSeconds(1)
        .RepeatForever())
    .Build();

await scheduler.ScheduleJob(job, trigger);

// Wait a bit before terminating.
await Task.Delay(10 * 1000);

await scheduler.Shutdown(waitForJobsToComplete: true);
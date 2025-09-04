using JobScheduling.Persistence.Application.Extensions;
using Quartz;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);
builder.Services.AddQuartz(options =>
{
    options.UsePersistentStore(persistenceOptions =>
    {
        persistenceOptions.UseSqlServer(builder.Configuration.GetDatabaseConnectionString());
        
        persistenceOptions.UseProperties = true;
        persistenceOptions.UseSystemTextJsonSerializer();
    });
});

var app = builder.Build();

app.MapHealthChecks("health");

app.Run();
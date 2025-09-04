using JobScheduling.Persistence.Application;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(o => o
    .UseSqlServer(builder.Configuration.GetConnectionString("Database")));

builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapHealthChecks("health");

app.Run();
using WebApplication1.Logic;
using WebApplication1.Models;
using WebApplication1.Services;
using WebApplication1.Services.Queue;

var builder = WebApplication.CreateBuilder(args);

var env = builder.Environment;
builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true);
//add hosted services
builder.Services.AddHostedService<TaskSchedulerService>();
builder.Services.AddHostedService<WorkerService>();


// Add services to the container.
builder.Services.AddSingleton<Settings>();
builder.Services.AddSingleton<TaskProcessor>();
builder.Services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
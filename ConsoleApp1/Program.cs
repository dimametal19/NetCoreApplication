using ConsoleApp1.Logic;
using ConsoleApp1.Models;
using ConsoleApp1.Services;
using ConsoleApp1.Services.Queue;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = new HostBuilder()
    .ConfigureAppConfiguration(configurationBuilder =>
    {
        configurationBuilder.AddJsonFile("config.json");
        configurationBuilder.AddCommandLine(args);
    })
    .ConfigureLogging((loggingBuilder =>
    {
        loggingBuilder.AddConsole();
        loggingBuilder.AddDebug();
    }))
    .ConfigureServices((services =>
    {
        services.AddHostedService<TaskSchedulerService>();
        services.AddHostedService<WorkerService>();
        
        services.AddSingleton<Settings>();
        services.AddSingleton<TaskProcessor>();
        services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
    }));

await builder.RunConsoleAsync();
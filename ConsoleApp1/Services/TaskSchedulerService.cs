using ConsoleApp1.Logic;
using ConsoleApp1.Models;
using ConsoleApp1.Services.Queue;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ConsoleApp1.Services;

public class TaskSchedulerService : IHostedService, IDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Settings _settings;
    private Timer timer;
    private readonly ILogger<TaskSchedulerService> _logger;
    private readonly object syncRoot = new();
    private readonly Random _random = new();
    

    public TaskSchedulerService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _settings = serviceProvider.GetRequiredService<Settings>();
        _logger = serviceProvider.GetRequiredService<ILogger<TaskSchedulerService>>();
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        var interval = _settings.RunInterval;
        if (interval == 0)
        {
            _logger.LogWarning("Interval empty. Set it by default: 60 sec");
            interval = 60;
        }
        timer = new Timer(
            (e) => ProcessTask(),
            null,
            TimeSpan.Zero,
            TimeSpan.FromSeconds(interval));
        return Task.CompletedTask;
    }

    private void ProcessTask()
    {
        if (Monitor.TryEnter(syncRoot))
        {
            _logger.LogInformation("Process task started");
            for (int i = 0; i < 20; i++)
            {
                DoWork();
            }
            _logger.LogInformation("Process task finished");
            Monitor.Exit(syncRoot);
        }
        else
        {
            _logger.LogInformation("Processing is currently is progress");
        }
    }

    private void DoWork()
    {
        var number = _random.Next(30);
        var processor = _serviceProvider.GetRequiredService<TaskProcessor>();
        var queue = _serviceProvider.GetRequiredService<IBackgroundTaskQueue>();
        
        queue.QueueBackgroundWorkItem(token => processor.CalculateAsync(number, token));
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        timer?.Dispose();
    }
}
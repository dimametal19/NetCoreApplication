using System.ComponentModel;
using ConsoleApp1.Models;
using ConsoleApp1.Services.Queue;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ConsoleApp1.Services;

public class WorkerService : BackgroundService
{
    private readonly IBackgroundTaskQueue _backgroundTaskQueue;
    private readonly ILogger<WorkerService> _logger;
    private readonly Settings _settings;

    public WorkerService(IBackgroundTaskQueue backgroundTaskQueue, ILogger<WorkerService> logger, Settings settings)
    {
        _backgroundTaskQueue = backgroundTaskQueue;
        _logger = logger;
        _settings = settings;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var workersCount = _settings.WorkersCount;
        var workers = Enumerable.Range(0, workersCount).Select(num => RunInstance(num, stoppingToken));
        await Task.WhenAll(workers);
    }

    private async Task RunInstance(int num, CancellationToken stoppingToken)
    {
        _logger.LogInformation("{Num} is starting", num);
        while (!stoppingToken.IsCancellationRequested)
        {
            var workItem = await _backgroundTaskQueue.DequeueAsync(stoppingToken);
            try
            {
                _logger.LogInformation("{Num}: Processing. Queue size: {Size}", num, _backgroundTaskQueue.Size);
                await workItem(stoppingToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Num}: Error", num);
            }
        }
        _logger.LogInformation("{Num} is stopping", num);
    }
}
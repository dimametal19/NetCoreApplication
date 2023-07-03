using static System.String;

namespace WebApplication1.Logic;

public class TaskProcessor
{
    private readonly ILogger<TaskProcessor> _logger;

    public TaskProcessor(ILogger<TaskProcessor> logger)
    {
        _logger = logger;
    }

    public async Task CalculateAsync(int number, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        var result = new List<int>();
        for (int i = 0; i < number; i++)
        {
            result.Add(i);
        }
        
        _logger.LogInformation("Task finished. Result:{Result}", Join(" ", result));
        await Task.Delay(1000, token);
    }
}
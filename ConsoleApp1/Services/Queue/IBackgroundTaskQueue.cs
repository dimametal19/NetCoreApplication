namespace ConsoleApp1.Services.Queue;

public interface IBackgroundTaskQueue
{
    int Size { get; }
    void QueueBackgroundWorkItem(Func<CancellationToken, Task> workItem);
    Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken);
}
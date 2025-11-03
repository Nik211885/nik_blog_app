using System.Threading.Channels;
namespace WebApi.Services.BackgroundTaskQueue;

public class BackgroundTaskQueue : IBackgroundTaskQueue
{
    private readonly Channel<Func<CancellationToken, Task>> _queue = Channel.CreateUnbounded<Func<CancellationToken, Task>>();
    /// <summary>
    ///     Add work item to queue
    /// </summary>
    /// <param name="workItem">work item</param>
    public async Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken = default)
    {
        var workItem = await _queue.Reader.ReadAsync(cancellationToken);
        return workItem;
    }
    /// <summary>
    ///     Get work item from queue
    /// </summary>
    /// <param name="cancellationToken">token to cancellation action</param>
    /// <returns>
    ///     Return func is work item
    /// </returns>
    public void QueueBackgoundWorkItem(Func<CancellationToken, Task> workItem)
    {
        ArgumentNullException.ThrowIfNull(workItem);
        _queue.Writer.TryWrite(workItem);
    }
}

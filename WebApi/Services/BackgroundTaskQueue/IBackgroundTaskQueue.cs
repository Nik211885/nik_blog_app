
namespace WebApi.Services.BackgroundTaskQueue;

public interface IBackgroundTaskQueue
{
    /// <summary>
    ///     Add work item to queue
    /// </summary>
    /// <param name="workItem">work item</param>
    void QueueBackgoundWorkItem(Func<CancellationToken, Task> workItem);
    /// <summary>
    ///     Get work item from queue
    /// </summary>
    /// <param name="cancellationToken">token to cancellation action</param>
    /// <returns>
    ///     Return func is work item
    /// </returns>
    Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken = default);
}

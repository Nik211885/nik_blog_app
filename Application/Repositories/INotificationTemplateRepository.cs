using Application.Entities;

namespace Application.Repositories;

public interface INotificationTemplateRepository
    : IRepositoryBase<NotificationTemplate>
{
    /// <summary>
    ///  Find template notification by id
    /// </summary>
    /// <param name="id">id for notification template</param>
    /// <param name="cancellationToken">token to cancellation action</param>
    /// <returns>
    ///     Return  notification template when match id otherwise return null value
    /// </returns>
    Task<NotificationTemplate?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);  
    /// <summary>
    ///  Get notification by code
    /// </summary>
    /// <param name="code">code template want to find</param>
    /// <param name="cancellationToken">token to cancellation action</param>
    /// <returns>
    ///     Return  notification template when match notification code return null value
    /// </returns>
    Task<NotificationTemplate?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
}
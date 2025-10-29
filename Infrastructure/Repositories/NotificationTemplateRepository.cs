using Application.Entities;
using Application.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class NotificationTemplateRepository
    : RepositoryBase<NotificationTemplate>, INotificationTemplateRepository
{
    private readonly ApplicationDbContext _dbContext;

    public NotificationTemplateRepository(ApplicationDbContext dbContext) 
        : base(dbContext)
    {
        _dbContext = dbContext;
    }
    /// <summary>
    ///  Find template notification by id
    /// </summary>
    /// <param name="id">id for notification template</param>
    /// <param name="cancellationToken">token to cancellation action</param>
    /// <returns>
    ///     Return  notification template when match id otherwise return null value
    /// </returns>
    public async Task<NotificationTemplate?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        NotificationTemplate? notificationTemplate = await _dbContext.NotificationTemplates
            .Where(x=>x.Id == id)
            .Include(x=>x.Arguments)
            .FirstOrDefaultAsync(cancellationToken);
        return notificationTemplate;
    }
    /// <summary>
    ///  Get notification by code
    /// </summary>
    /// <param name="code">code template want to find</param>
    /// <param name="cancellationToken">token to cancellation action</param>
    /// <returns>
    ///     Return  notification template when match notification code return null value
    /// </returns>
    public async Task<NotificationTemplate?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        NotificationTemplate? notificationTemplate = await _dbContext.NotificationTemplates
            .Where(x=>x.Code == code)
            .Include(x => x.Arguments)
            .FirstOrDefaultAsync(cancellationToken);
        return notificationTemplate;
    }
}
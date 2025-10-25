using Application.Entities;
using Application.Repositories;
using Infrastructure.Data;

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
}
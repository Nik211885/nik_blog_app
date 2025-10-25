using Application.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.Services.NotificationTemplateManager;

public class NotificationTemplateServices
{
    private readonly ILogger<NotificationTemplateServices> _logger;
    private readonly INotificationTemplateRepository _repository;

    public NotificationTemplateServices(ILogger<NotificationTemplateServices> logger, 
        INotificationTemplateRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }
}
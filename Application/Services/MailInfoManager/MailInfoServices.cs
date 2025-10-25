using Application.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.Services.MailInfoManager;

public class MailInfoServices
{
    private readonly ILogger<MailInfoServices> _logger;
    private readonly IMalInfoRepository _malInfoRepository;

    public MailInfoServices(ILogger<MailInfoServices> logger, IMalInfoRepository malInfoRepository)
    {
        _logger = logger;
        _malInfoRepository = malInfoRepository;
    }
}
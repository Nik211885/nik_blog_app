using Application.Entities;
using Application.Exceptions;
using Application.Repositories;
using Application.Services.MailInfoManager.Requests;
using Microsoft.Extensions.Logging;

namespace Application.Services.MailInfoManager;

public class MailInfoServices
{
    private readonly ILogger<MailInfoServices> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public MailInfoServices(ILogger<MailInfoServices> logger,  IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    /// <summary>
    ///     Create new instance for mail info
    /// </summary>
    /// <param name="request">Information to create instance mail info</param>
    /// <param name="cancellationToken">token to cancellation action</param>
    /// <returns>
    ///     Return mail info response when create mail info success
    /// </returns>
    public async Task<MailInfo> CreateMailInfoAsync(CreateMailInfoRequest request,
        CancellationToken cancellationToken = default)
    {
        MailInfo? mailInfo = await _unitOfWork.MailInfoRepository
            .GetMailInfoByEmailIdAsync(request.EmailId, cancellationToken);
        ThrowHelper.ThrowBusinessErrorWhenExitsItem(mailInfo, MailInfoConstMessage.EmailIdHasExits);
        
        NotificationTemplate? notificationTemplate = await _unitOfWork
            .NotificationTemplateRepository.GetByIdAsync(request.TemplateId, cancellationToken);
        ThrowHelper.ThrowWhenNotFoundItem(notificationTemplate, MailInfoConstMessage.NotificationTemplateNotFound);
        
        // check just notification chanel is mail just has added mail info
        MailInfoBusinessRule.CreateRule(notificationTemplate).MailChanelMustAddToTemplate();
        mailInfo = request.MapToMailInfo();
        // if template has added mail info before override for this mail info
        notificationTemplate.MailInfo = mailInfo;
        
        _unitOfWork.MailInfoRepository.Add(mailInfo);
        _unitOfWork.NotificationTemplateRepository.Update(notificationTemplate);
        await _unitOfWork.SaveChangeAsync(cancellationToken);
        return mailInfo;
    }
    /// <summary>
    ///     Update mail info
    /// </summary>
    /// <param name="request">request need  update mail info</param>
    /// <param name="cancellationToken">token to cancellation action</param>
    /// <returns>
    ///     Return mail info response when update mail info success and
    ///     throw not found exception when not found mail info with id mail info
    /// </returns>
    public async Task<MailInfo> UpdateMailInfoAsync(UpdateMailInfoRequest request,
        CancellationToken cancellationToken = default)
    {
        MailInfo? mailInfo = await _unitOfWork.MailInfoRepository
            .GetMailInfoByIdAsync(request.MailInfoId, cancellationToken);
        ThrowHelper.ThrowWhenNotFoundItem(mailInfo, MailInfoConstMessage.MailInfoNotFound);

        NotificationTemplate? notificationTemplate = await _unitOfWork
            .NotificationTemplateRepository.GetByIdAsync(request.TemplateId, cancellationToken);
        ThrowHelper.ThrowWhenNotFoundItem(notificationTemplate, MailInfoConstMessage.NotificationTemplateNotFound);
        // check just notification chanel is mail just has added mail info
        MailInfoBusinessRule.CreateRule(notificationTemplate).MailChanelMustAddToTemplate();
        // if mail info is active override for all template
        // else if template is inactive override for template but if template is active you can't change to template
        if (!request.IsActive && notificationTemplate.IsActive)
        {
            ThrowHelper.ThrowWhenBusinessError(MailInfoConstMessage.MailInfoInActiveCanNotAddToTemplateActive);
        }
        notificationTemplate.MailInfo = mailInfo;
        request.MapToMailInfo(mailInfo);
        _unitOfWork.MailInfoRepository.Update(mailInfo);
        await _unitOfWork.SaveChangeAsync(cancellationToken);
        return mailInfo;
    }
    
}
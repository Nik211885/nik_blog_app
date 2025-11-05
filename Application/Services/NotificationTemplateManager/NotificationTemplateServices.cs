using System.Dynamic;
using Application.Adapters;
using Application.Entities;
using Application.Enums;
using Application.Exceptions;
using Application.Extensions;
using Application.Repositories;
using Application.Services.NotificationTemplateManager.Requests;
using Application.Services.NotificationTemplateManager.Responses;
using Microsoft.Extensions.Logging;

namespace Application.Services.NotificationTemplateManager;

public class NotificationTemplateServices
{
    private readonly ILogger<NotificationTemplateServices> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailServices _emailServices;
    private readonly IConnectionQueryService _connectionQueryServices;

    public NotificationTemplateServices(ILogger<NotificationTemplateServices> logger,
     IEmailServices emailServices, IUnitOfWork unitOfWork, IConnectionQueryService connectionQueryServices)
    {
        _logger = logger;
        _emailServices = emailServices;
        _unitOfWork = unitOfWork;
        _connectionQueryServices = connectionQueryServices;
    }
    /// <summary>
    ///     Create notification template
    /// </summary>
    /// <param name="request">information for request need to created</param>
    /// <param name="cancellationToken">token need to cancellation action</param>
    /// <returns>
    ///     Return notification response after created template success
    /// </returns>
    public async Task<NotificationTemplate> CreateNotificationTemplateAsync(
        CreateNotificationTemplateRequest request, CancellationToken cancellationToken = default)
    {
        NotificationTemplate? notificationTemplate = await _unitOfWork.NotificationTemplateRepository
            .GetByCodeAsync(request.Code, cancellationToken);
        ThrowHelper.ThrowBusinessErrorWhenExitsItem(notificationTemplate, NotificationTemplateConstMessage.HasExitsCode);

        // get all param user has passed 
        List<Arguments>? arguments = null;
        if (request.ArgumentsId is not null && request.ArgumentsId.Count != 0)
        {
            IEnumerable<Arguments> argumentsById = await _unitOfWork.ArgumentRepository.GetByIdsAsync(cancellationToken, [.. request.ArgumentsId]);
            arguments = [.. argumentsById];
        }
        // you can check if argument must equals user pass to request
        notificationTemplate = request.MapToNotificationTemplate(arguments);
        NotificationTemplateBusinessRule.CreateRule(notificationTemplate)
            .ContentCanNotEmpty()
            .CheckChanelNotificationForTemplateType()
            .CompareArgumentInContent();
        // compare param with params user has passed it if arguments in template bigger or
        // smaller arguments collection throw exceptions template invalid
        await MailInfoMustActiveToAddTemplateAsync(request.MailInfoId, cancellationToken);
        _unitOfWork.NotificationTemplateRepository.Add(notificationTemplate);
        await _unitOfWork.SaveChangeAsync(cancellationToken);
        return notificationTemplate;
    }
    /// <summary>
    ///     Update notification template
    /// </summary>
    /// <param name="request">infomation need to update notification template</param>
    /// <param name="cancellationToken">token to cacellation action update</param>
    /// <returns>
    ///     Return notification template response after updated success
    /// </returns>
    public async Task<NotificationTemplate> UpdateNotificationTemplateAsync(UpdateNotificationTemplateRequest request,
    CancellationToken cancellationToken = default)
    {
        NotificationTemplate? notificationTemplate = await _unitOfWork.NotificationTemplateRepository
                                                        .GetByIdAsync(request.NotificationTemplateId, cancellationToken);
        ThrowHelper.ThrowWhenNotFoundItem(notificationTemplate, NotificationTemplateConstMessage.NotificationTemplateNotFound);
        NotificationTemplate? notificationTemplateByCode = await _unitOfWork.NotificationTemplateRepository
            .GetByCodeAsync(request.Code, cancellationToken);
        ThrowHelper.ThrowBusinessErrorWhenExitsItem(notificationTemplate, NotificationTemplateConstMessage.HasExitsCode);
        if (notificationTemplateByCode is not null && notificationTemplateByCode.Id != notificationTemplate.Id)
        {
            ThrowHelper.ThrowWhenBusinessError(NotificationTemplateConstMessage.HasExitsCode);
        }
        List<Arguments>? arguments = null;
        if (request.ArgumentsId is not null && request.ArgumentsId.Count != 0)
        {
            IEnumerable<Arguments> argumentsById = await _unitOfWork.ArgumentRepository.GetByIdsAsync(cancellationToken, [.. request.ArgumentsId]);
            arguments = [.. argumentsById];
        }
        request.MapToNotificationTemplate(notificationTemplate, arguments);

        NotificationTemplateBusinessRule.CreateRule(notificationTemplate)
            .ContentCanNotEmpty()
            .CheckChanelNotificationForTemplateType()
            .CompareArgumentInContent();
        // if not active just check all notification template must have one active with services type
        // because services type with notification template i think it will very small so, i load all data to memory and check it into memory
        if (!request.IsActive)
        {
            IEnumerable<NotificationTemplate> notificationTemplateByServicesType = await _unitOfWork
                .NotificationTemplateRepository.GetByServiceTypeAsync(request.NotificationServicesType, cancellationToken);
            if (!notificationTemplateByServicesType.Any(x => x.IsActive))
            {
                ThrowHelper.ThrowWhenBusinessError(NotificationTemplateConstMessage.ServicesTypeMustHaveOneOrMoreTemplateActive);
            }
        }
        else
        {
            await MailInfoMustActiveToAddTemplateAsync(request.MailInfoId, cancellationToken);
        }

        _unitOfWork.NotificationTemplateRepository.Update(notificationTemplate);
        await _unitOfWork.SaveChangeAsync(cancellationToken);
        return notificationTemplate;
    }
    /// <summary>
    ///     Send mail with template specific
    /// </summary>
    /// <param name="templateId">id for template</param>
    /// <param name="to">email id for to</param>
    /// <param name="nameTo">name for email to</param>
    /// <param name="cancellationToken">token to cancellation action</param>
    /// <param name="param">param support for query in arguments</param>
    /// <param name="defaultParamsInMessage">param support for query in arguments</param>
#pragma warning disable CA1068 // CancellationToken parameters must come last
    public async Task SendMailWithTemplateAsync(Guid templateId, string to, string? nameTo,
#pragma warning restore CA1068 // CancellationToken parameters must come last
    CancellationToken cancellationToken = default, Dictionary<string, object>? param = null,
    Dictionary<string, object>? defaultParamsInMessage = null)
    {
        NotificationTemplate? notificationTemplate = await _unitOfWork.NotificationTemplateRepository
                                                .GetByIdAsync(templateId, cancellationToken);
        ThrowHelper.ThrowWhenNotFoundItem(notificationTemplate, NotificationTemplateConstMessage.NotificationTemplateNotFound);
        // get mail info to send mail 
        NotificationTemplateBusinessRule.CreateRule(notificationTemplate).CheckMailInfoForTemplatePushMail();
        MailInfo? mailInfo = await _unitOfWork.MailInfoRepository.GetMailInfoByIdAsync((Guid)notificationTemplate.MailInfoId!, cancellationToken);
        ThrowHelper.ThrowWhenNotFoundItem(mailInfo, NotificationTemplateConstMessage.CanNotFindMailInfoToSendTemplate);
        // get message and aggregate into notification content
        var paramenters = new ExpandoObject() as IDictionary<string, object>;
        paramenters["email"] = to;
        paramenters["fullName"] = nameTo ?? string.Empty;
        if (param is not null)
        {
            foreach (var key in param.Keys)
            {
                paramenters[key] = param[key];
            }
        }
        var paramInMailContent = new Dictionary<string, object?>();
        if (notificationTemplate.Arguments is not null)
        {
            foreach (var a in notificationTemplate.Arguments)
            {
                var valueOfQuery = await _connectionQueryServices.QueryFirstOrDefaultAsync<object>(a.Query, param);
                paramInMailContent.Add(a.Code, valueOfQuery);
            }
        }
        if (defaultParamsInMessage is not null)
        {
            foreach (var keyOfParamMainContent in defaultParamsInMessage.Keys)
            {
                paramInMailContent[keyOfParamMainContent] = defaultParamsInMessage[keyOfParamMainContent];
            }
        }
        bool IsHtmlBody = false;
        string content = string.Empty;
        if (notificationTemplate.ContentHtml is not null)
        {
            IsHtmlBody = true;
            content = notificationTemplate.ContentHtml;
        }
        if (notificationTemplate.ContentText is not null)
        {
            IsHtmlBody = false;
            content = notificationTemplate.ContentText;
        }
        content = content.InterpolateStringWithDictionaryParam(paramInMailContent);
        await _emailServices.SendEmailAsync(mailInfo.EmailId, to, content, notificationTemplate.Subject,
        IsHtmlBody, mailInfo.Host, mailInfo.Port, mailInfo.EnableSsl, mailInfo.Password, nameTo, mailInfo.Name);
    }
    /// <summary>
    ///     Send notification template with services type
    /// </summary>
    /// <param name="notificationServicesType">services for notification template</param>
    /// <param name="to">email id for to</param>
    /// <param name="nameTo">name for email to</param>
    /// <param name="cancellationToken">token to cancellation action</param>
    /// <param name="param">param support for query in arguments</param>
    /// <param name="defaultParamsInMessage">param support for query in arguments</param>
#pragma warning disable CA1068 // CancellationToken parameters must come last
    public async Task SendMailWithTemplateServiceAsync(NotificationServicesType notificationServicesType, string to,
#pragma warning restore CA1068 // CancellationToken parameters must come last
    string? nameTo, CancellationToken cancellationToken = default, Dictionary<string, object>? param = null,
    Dictionary<string, object>? defaultParamsInMessage = null)
    {
        IEnumerable<NotificationTemplate> notificationTemplatesByServices = await _unitOfWork.NotificationTemplateRepository
                                                                        .GetByServiceTypeAsync(notificationServicesType, cancellationToken);
        foreach (var n in notificationTemplatesByServices)
        {
            await SendMailWithTemplateAsync(n.Id, to, nameTo, cancellationToken, param, defaultParamsInMessage);
        }
    }

    /// <summary>
    ///   Check if mail info is exits mail info must active
    /// </summary>
    /// <param name="mailInfoID">id for mail info</param>
    /// <param name="cancellationToken">token to cancellation action</param>
    private async Task MailInfoMustActiveToAddTemplateAsync(Guid? mailInfoID, CancellationToken cancellationToken = default)
    {
        if (mailInfoID is not null)
        {
            MailInfo? mailInfo = await _unitOfWork.MailInfoRepository.GetMailInfoByIdAsync((Guid)mailInfoID, cancellationToken);
            ThrowHelper.ThrowWhenNotFoundItem(mailInfo, NotificationTemplateConstMessage.CantNotFindMailInfoToAddTemplate);
            if (!mailInfo.IsActive)
            {
                ThrowHelper.ThrowWhenBusinessError(NotificationTemplateConstMessage.CantNotFindMailInfoToAddTemplate);
            }
        }
    }
}
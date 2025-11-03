using System.ComponentModel.DataAnnotations;
using Application.Entities;

namespace Application.Services.MailInfoManager.Requests;

public class UpdateMailInfoRequest
{
    public Guid MailInfoId { get; set; }
    [Required(ErrorMessage = MailInfoConstMessage.HostIsRequired)]
    public string Host { get; init; } = string.Empty;
    public int Port { get; init; }
    [Required(ErrorMessage = MailInfoConstMessage.NameIsRequired)]
    public string Name { get; init; } = string.Empty;
    [Required(ErrorMessage = MailInfoConstMessage.EmailIdInValid)]
    public string EmailId { get; init; } = string.Empty;
    [Required(ErrorMessage = MailInfoConstMessage.UserNameIsRequired)]
    public string UserName { get; init; } = string.Empty;
    public bool IsActive { get; init; }
    public bool EnableSsl { get; init; }
    public Guid TemplateId {get; init; }
}

public static class UpdateMailInfoRequestExtension
{
    public static void MapToMailInfo(this UpdateMailInfoRequest request, MailInfo mailInfo)
    {
        mailInfo.Host = request.Host;
        mailInfo.Port = request.Port;
        mailInfo.Name = request.Name;
        mailInfo.IsActive = request.IsActive;
        mailInfo.EmailId = request.EmailId;
        mailInfo.UserName = request.UserName;
        mailInfo.EnableSsl = request.EnableSsl;
    }
}
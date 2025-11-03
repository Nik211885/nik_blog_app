using System.ComponentModel.DataAnnotations;
using Application.Entities;

namespace Application.Services.MailInfoManager.Requests;

public class CreateMailInfoRequest
{
    [Required(ErrorMessage = MailInfoConstMessage.HostIsRequired)]
    public string Host { get; init; } = string.Empty;
    public int Port { get; init; }
    [Required(ErrorMessage = MailInfoConstMessage.NameIsRequired)]
    public string Name { get; init; } = string.Empty;
    [EmailAddress(ErrorMessage = MailInfoConstMessage.EmailIdInValid)]
    public string EmailId { get; init; } = string.Empty;
    [Required(ErrorMessage = MailInfoConstMessage.UserNameIsRequired)]
    public string UserName { get; init; } = string.Empty;
    [Required(ErrorMessage = MailInfoConstMessage.ApplicationPasswordIsRequired)]
    public string Password { get; init; } = string.Empty;
    public bool EnableSsl { get; init; }
    public Guid TemplateId { get; init; }
}

public static class CreateMailInfoRequestExtension
{
    public static MailInfo MapToMailInfo(this CreateMailInfoRequest request)
    {
        return new MailInfo()
        {
            Host = request.Host,
            Port = request.Port,
            Name = request.Name,
            EmailId = request.EmailId,
            UserName = request.UserName,
            Password = request.Password,
            EnableSsl = request.EnableSsl
        };
    }
}
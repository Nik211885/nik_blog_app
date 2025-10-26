using Application.Entities;

namespace Application.Services.MailInfoManager.Responses;

public class MailInfoResponse 
{
    public Guid Id { get; init; }
    public string Host { get; init; } = string.Empty;
    public int  Port { get; init; }
    public string Name { get; init; } = string.Empty;
    public string EmailId { get; init; } = string.Empty;
    public string UserName { get; init; } = string.Empty;
    public bool EnableSsl { get; init; }
    public bool IsActive { get; init; } = true;
}

internal static class MailInfoResponseExtensions
{
    public static MailInfoResponse MapToResponse(this MailInfo mailInfo)
    {
        return new MailInfoResponse()
        {
            Id = mailInfo.Id,
            Host = mailInfo.Host,
            Port = mailInfo.Port,
            Name = mailInfo.Name,
            EmailId = mailInfo.EmailId,
            UserName = mailInfo.UserName,
            EnableSsl = mailInfo.EnableSsl,
            IsActive = mailInfo.IsActive
        };
    }
}
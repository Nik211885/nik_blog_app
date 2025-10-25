using Application.Entities;

namespace Application.Services.MailInfoManager.Responses;

public class MailInfoResponse 
{
    
}

internal static class MailInfoResponseExtensions
{
    public static MailInfoResponse MapToResponse(this MailInfo mailInfo)
    {
        return new MailInfoResponse()
        {

        };
    }
}
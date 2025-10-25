using Application.Entities;

namespace Application.Services.MailInfoManager.Requests;

public class CreateMailInfoRequest
{
    
}

internal static class CreateMailInfoRequestExtension
{
    public static MailInfo MapToMailInfo(this CreateMailInfoRequest request)
    {
        return new MailInfo()
        {

        };
    }
}
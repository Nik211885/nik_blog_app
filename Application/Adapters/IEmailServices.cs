namespace Application.Adapters;
/// <summary>
///  Services support for send message 
/// </summary>
public interface IEmailServices
{
    /// <summary>
    ///  Send email with template id with argument pass for form 
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="content"></param>
    /// <param name="subject"></param>
    /// <param name="contentHtml"></param>
    /// <param name="host"></param>
    /// <param name="port"></param>
    /// <param name="useSsl"></param>
    /// <param name="applicationPassword"></param>
    /// <param name="nameTo"></param>
    /// <param name="nameFrom"></param>
    Task SendEmailAsync(string from, string to, string? content, string subject, string? contentHtml, string host,
        int port, bool useSsl, string applicationPassword, string? nameTo = null, string? nameFrom = null);
}
namespace Application.Adapters;
/// <summary>
///  Services support for send message 
/// </summary>
public interface IEmailServices
{
    /// <summary>
    ///  Send email with template id with argument pass for form 
    /// </summary>
    /// <param name="templateId"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    Task SendAsync(Guid templateId, params string[] args);
}
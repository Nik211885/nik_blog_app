using Application.Adapters;

namespace Infrastructure.Adapters;

/// <summary>
///  Email services
/// </summary>
public class EmailServices : IEmailServices
{
    /// <summary>
    ///  Send email with template and arguments fit for template argument
    /// </summary>
    /// <param name="templateId"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public Task SendAsync(Guid templateId, params string[] args)
    {
        throw new NotImplementedException();
    }
}
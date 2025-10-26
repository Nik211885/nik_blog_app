using Application.Entities;

namespace Application.Repositories;

public interface IMalInfoRepository : IRepositoryBase<MailInfo>
{
    /// <summary>
    ///  Get mail info by id 
    /// </summary>
    /// <param name="id">id for mail info</param>
    /// <param name="cancellationToken">token to cancellation action</param>
    /// <returns>
    ///     Return mail infor when match id otherwsise is null value
    /// </returns>
    Task<MailInfo?> GetMailInfoByIdAsync(Guid id, CancellationToken cancellationToken = default);
    /// <summary>
    ///      Get mail info by email id
    /// </summary>
    /// <param name="emailId">id for email it is email address</param>
    /// <param name="cancellationToken">token to cancellation action</param>
    /// <returns>
    ///     Return mail info when match email id otherwise is null value 
    /// </returns>
    Task<MailInfo?> GetMailInfoByEmailIdAsync(string emailId, CancellationToken cancellationToken = default);
}
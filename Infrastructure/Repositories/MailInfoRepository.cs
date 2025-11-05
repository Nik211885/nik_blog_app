using Application.Entities;
using Application.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class MailInfoRepository
    : RepositoryBase<MailInfo>, IMalInfoRepository
{
    private readonly ApplicationDbContext _dbContext;

    public MailInfoRepository(ApplicationDbContext dbContext)
        : base(dbContext)
    {
        _dbContext = dbContext;
    }
    /// <summary>
    ///  Get mail info by id 
    /// </summary>
    /// <param name="id">id for mail info</param>
    /// <param name="cancellationToken">token to cancellation action</param>
    /// <returns>
    ///     Return mail infor when match id otherwsise is null value
    /// </returns>
    public async Task<MailInfo?> GetMailInfoByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        MailInfo? mailInfo = await _dbContext.MailInfos
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        return mailInfo;
    }
    /// <summary>
    ///      Get mail info by email id
    /// </summary>
    /// <param name="emailId">id for email it is email address</param>
    /// <param name="cancellationToken">token to cancellation action</param>
    /// <returns>
    ///     Return mail info when match email id otherwise is null value 
    /// </returns>
    public async Task<MailInfo?> GetMailInfoByEmailIdAsync(string emailId, CancellationToken cancellationToken = default)
    {
        MailInfo? mailInfo = await _dbContext.MailInfos
            .FirstOrDefaultAsync(x => x.EmailId == emailId, cancellationToken);
        return mailInfo;
    }
}
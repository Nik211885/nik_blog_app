using Application.Entities;
using Application.Repositories;
using Infrastructure.Data;

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
}
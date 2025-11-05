using Application.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Repositories;

/// <summary>
///  In here i has manual created instance for repository
///  Otherwise you can use reflection to create repository it is a magic way and in this file
///  don't have miss open close principle
///  Otherwise you can pass injection is services provider and pass to di container
///  will have manager lifecycle for repository, and it will easy to mock,
///  but you need specific and add all repository to di container
/// </summary>
public class RepositoryWrapper : IWrapperRepositories
{
    private readonly IServiceProvider _serviceProvider;
    protected RepositoryWrapper(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    /// <summary>
    ///  User repository in di container management
    /// </summary>
    public IUserRepository UserRepository => GetRepository<IUserRepository>();
    /// <summary>
    ///  Notification repository in di container management
    /// </summary>
    public INotificationTemplateRepository NotificationTemplateRepository => GetRepository<INotificationTemplateRepository>();
    /// <summary>
    ///  mail info repository in di container management
    /// </summary>
    public IMalInfoRepository MailInfoRepository => GetRepository<IMalInfoRepository>();
    /// <summary>
    ///  Get instance for argument repository in di container
    /// </summary>
    public IArgumentRepository ArgumentRepository => GetRepository<IArgumentRepository>();
    /// <summary>
    ///  Get instance for login provider repository in di container
    /// </summary>
    public ILoginProviderRepository LoginProviderRepository => GetRepository<ILoginProviderRepository>();

    /// <summary>
    ///  Get instance for repository in services provider process case when dont have instance in container
    /// </summary>
    /// <typeparam name="TInstanceForRepository">Type Instance for repository</typeparam>
    /// <returns>
    ///     Return instance for repository has specific
    /// </returns>
    private TInstanceForRepository GetRepository<TInstanceForRepository>()
    {
        TInstanceForRepository? repository = _serviceProvider.GetService<TInstanceForRepository>();
        ArgumentNullException.ThrowIfNull(repository);
        return repository;
    }
}
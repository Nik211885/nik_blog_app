namespace Application.Repositories;
/// <summary>
///  Wrapper all repository
/// </summary>
public interface IWrapperRepositories
{
    /// <summary>
    ///     Repository for user
    /// </summary>
    IUserRepository UserRepository { get; }
    /// <summary>
    ///  Repository for notification template
    /// </summary>
    INotificationTemplateRepository NotificationTemplateRepository { get; }
    /// <summary>
    ///  Repository for mail info 
    /// </summary>
    IMalInfoRepository MailInfoRepository { get; }
    /// <summary>
    ///  Repository for argument 
    /// </summary>
    IArgumentRepository ArgumentRepository { get; }
    /// <summary>
    ///  Repository for login provider
    /// </summary>
    ILoginProviderRepository LoginProviderRepository { get; }
}
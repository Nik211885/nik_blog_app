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
}
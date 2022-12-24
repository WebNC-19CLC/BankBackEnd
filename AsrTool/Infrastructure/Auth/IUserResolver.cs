namespace AsrTool.Infrastructure.Auth
{
  public interface IUserResolver
  {
    /// <summary>
    /// Get current active user.
    /// </summary>
    IUser CurrentUser { get; }

    /// <summary>
    /// Use the system user in next scope of codes.
    /// THIS IS NOT THREAD SAFE
    /// </summary>
    /// <returns></returns>
    ISystemUserScope UseSystemUser();
  }
}

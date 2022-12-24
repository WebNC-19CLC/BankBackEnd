namespace AsrTool.Infrastructure.Auth
{
  public class UserResolver : IUserResolver
  {
    public UserResolver(IUser normalUser)
    {
      CurrentUser = normalUser;
    }

    public IUser CurrentUser { get; private set; }

    public ISystemUserScope UseSystemUser()
    {
      return new SystemUserScope(this);
    }

    private class SystemUserScope : ISystemUserScope
    {
      private readonly UserResolver _userResolver;
      private readonly IUser _originalUser;

      public SystemUserScope(UserResolver userResolver)
      {
        _userResolver = userResolver;
        _originalUser = userResolver.CurrentUser;
        _userResolver.CurrentUser = new SystemUser();
      }

      public void Dispose()
      {
        _userResolver.CurrentUser = _originalUser;
      }
    }
  }
}

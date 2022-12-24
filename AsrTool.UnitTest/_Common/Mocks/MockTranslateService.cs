using AsrTool.Infrastructure.Common.Translation;
using AsrTool.Infrastructure.Domain.Enums;

namespace AsrTool.UnitTest._Common.Mocks
{
  public class MockTranslateService : ITranslateService
  {
    public string GetLocalizedData(AppLanguage language, string key, params string[] prefixes)
    {
      return key;
    }

    public string GetLocalizedData(AppLanguage language, string key, object info, params string[] prefixes)
    {
      return key;
    }

    public string GetLocalizedData(string key, params string[] prefixes)
    {
      return GetLocalizedData(AppLanguage.English, key);
    }

    public string GetLocalizedData(string key, object info, params string[] prefixes)
    {
      return GetLocalizedData(AppLanguage.English, key, info);
    }
  }
}

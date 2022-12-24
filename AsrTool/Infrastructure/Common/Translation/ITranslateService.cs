using AsrTool.Infrastructure.Domain.Enums;

namespace AsrTool.Infrastructure.Common.Translation
{
  public interface ITranslateService
  {
    public string GetLocalizedData(AppLanguage language, string key, params string[] prefixes);

    public string GetLocalizedData(AppLanguage language, string key, object info, params string[] prefixes);

    public string GetLocalizedData(string key, params string[] prefixes);

    public string GetLocalizedData(string key, object info, params string[] prefixes);
  }
}
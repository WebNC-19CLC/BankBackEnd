using System.Text.RegularExpressions;
using AsrTool.Infrastructure.Domain.Enums;
using AsrTool.Infrastructure.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AsrTool.Infrastructure.Common.Translation
{
  public class TranslateService : ITranslateService
  {
    private static readonly Dictionary<AppLanguage, JObject> s_localizeDictionary;

    private static readonly IReadOnlyDictionary<AppLanguage, string> s_localizationFileNameDictionary = new Dictionary<AppLanguage, string>
    {
      { AppLanguage.English, "en" },
      { AppLanguage.French, "fr" },
      { AppLanguage.German, "de" }
    };

    static TranslateService()
    {
      s_localizeDictionary = new Dictionary<AppLanguage, JObject>();
      var paths = GetLocalizationFiles();
      foreach (var language in Enum.GetValues<AppLanguage>())
      {
        var foundFile = s_localizationFileNameDictionary.TryGetValue(language, out var fileName);
        if (!foundFile)
        {
          continue;
        }

        s_localizeDictionary.Add(language, new JObject());
        var path = paths.FirstOrDefault(x => x.Contains($"{fileName}.json", StringComparison.OrdinalIgnoreCase));
        if (string.IsNullOrEmpty(path))
        {
          continue;
        }

        using var file = new StreamReader(File.OpenRead(path));
        s_localizeDictionary[language] = JsonConvert.DeserializeObject<JObject>(file.ReadToEnd());
      }
    }

    private static string[] GetLocalizationFiles()
    {
      var path = Path.Combine(PathHelper.GetExecutingPath(), "../../../ClientApp/src/assets/i18n");
      if (!Directory.Exists(path))
      {
        path = Path.Combine(PathHelper.GetExecutingPath(), "ClientApp/dist/assets/i18n");
      }

      return Directory.GetFiles(path, "*.json");
    }


    public string GetLocalizedData(AppLanguage language, string key, params string[] prefixes)
    {
      prefixes ??= new string[0];
      var root = s_localizeDictionary[language];
      foreach (var prefix in prefixes)
      {
        root.TryGetValue($"{prefix}", out var token);
        if (token == null || !(token is JObject jObject))
        {
          return string.Empty;
        }

        root = jObject;
      }

      return root.TryGetValue(key, out var value) ? $"{value}" : key;
    }

    public string GetLocalizedData(AppLanguage language, string key, object info, params string[] prefixes)
    {
      var localizedString = GetLocalizedData(language, key, prefixes);
      if (info == null)
      {
        return localizedString;
      }

      foreach (var propertyInfo in info.GetType().GetProperties())
      {
        var value = $"{propertyInfo.GetValue(info, null)}";
        var regex = new Regex(@$"\{{\{{( +{propertyInfo.Name} +|{propertyInfo.Name} +| +{propertyInfo.Name}|{propertyInfo.Name})\}}\}}");
        localizedString = regex.Replace(localizedString, value);
      }

      return localizedString;
    }

    public string GetLocalizedData(string key, params string[] prefixes)
    {
      return GetLocalizedData(AppLanguage.English, key, prefixes);
    }

    public string GetLocalizedData(string key, object info, params string[] prefixes)
    {
      return GetLocalizedData(AppLanguage.English, key, info, prefixes);
    }
  }
}
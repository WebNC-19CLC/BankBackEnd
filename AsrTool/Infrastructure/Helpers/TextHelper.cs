using System.Text.RegularExpressions;
using AsrTool.Infrastructure.Domain;

namespace AsrTool.Infrastructure.Helpers
{
  public static class TextHelper
  {
    public static string BuildContent<T>(string template, T model)
    {
      var replacements = new Dictionary<string, string>();
      var props = model.GetType().GetProperties()
          .Where(x => x.GetCustomAttributes(typeof(PlaceHolderAttribute), false).Any());

      foreach (var prop in props)
      {
        var attribute = prop.GetCustomAttributes(typeof(PlaceHolderAttribute), false)[0] as PlaceHolderAttribute;
        var variable = $"{Constants.Common.PLACEHOLDER_VARIABLE}{attribute.Name}";
        var value = prop.GetValue(model)?.ToString();
        replacements.Add(variable, value);
        if (!string.IsNullOrEmpty(attribute.ReplacementTemplate))
        {
          replacements.Add(string.Format(attribute.ReplacementTemplate, variable), value);
        }
      }

      var pattern = $"(?<placeholder>{string.Join("|", replacements.Keys.Select(x => Regex.Escape(x)))})";
      return Regex.Replace(template, pattern, m => replacements[m.Groups["placeholder"].Value], RegexOptions.ExplicitCapture);
    }
  }
}

using AsrTool.Infrastructure.Domain.Enums;

namespace AsrTool.Infrastructure.Extensions
{
  public static class RightExtension
  {
    public static string SerializePermission(this IEnumerable<Right> rights)
    {
      return string.Join(Constants.Auth.RIGHT_SEPARATOR, rights.Select(x => $"{x:d}"));
    }

    public static IEnumerable<Right> DeserializePermission(this string rightString)
    {
      if (string.IsNullOrEmpty(rightString))
      {
        return new List<Right>();
      }

      return rightString.Split(Constants.Auth.RIGHT_SEPARATOR, StringSplitOptions.RemoveEmptyEntries)
        .Select(x => (Right)int.Parse(x));
    }
  }
}
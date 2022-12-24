using System.Reflection;

namespace AsrTool.Infrastructure.Helpers
{
  public static class PathHelper
  {
    public static string GetExecutingPath()
    {
      return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
    }
  }
}
namespace AsrTool.Infrastructure.Helpers
{
  public static class StringHelper
  {
    /// <summary>
    ///     Add parentheses to string if it is not null or white space
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static string AddParentheses(string source)
    {
      if (string.IsNullOrWhiteSpace(source))
      {
        return source;
      }

      return $"({source})";
    }

    public static string[] FilterAllNullOrWhiteSpace(params string[] strings)
    {
      return strings.Where(str => !string.IsNullOrEmpty(str)).ToArray();
    }

    public static string GetFullName(string firstName, string lastName)
    {
      return string.Join(Constants.Model.NAME_SEPARATOR,
                         FilterAllNullOrWhiteSpace(firstName, lastName));
    }

    public static string GetFullName(string firstName, string lastName, string visa)
    {
      return string.Join(Constants.Model.NAME_SEPARATOR,
                         FilterAllNullOrWhiteSpace(firstName, lastName,
                         AddParentheses(visa)));
    }

    public static string JoinNewLine(this string[] strings)
    {
      return string.Join(Environment.NewLine, strings);
    }

    public static string GetTimeRange(TimeSpan? from, TimeSpan? to)
    {
      return string.Join(Constants.Model.TIME_RANGE_SEPARATOR,
                         from.HasValue ? from.Value.ToString(@"hh\:mm") : Constants.Model.DEFAULT,
                         to.HasValue ? to.Value.ToString(@"hh\:mm") : Constants.Model.DEFAULT);
    }

    public static string GetTimeString(TimeSpan? time)
    {
      return time.HasValue ? time.Value.ToString(@"hh\:mm") : Constants.Model.DEFAULT;
    }
  }
}
using TimeZoneConverter;

namespace AsrTool.Infrastructure.Helpers
{
  public static class DateTimeHelper
  {
    public static DateTime? GetLocalTime(DateTime? utcDateTime, string timezoneId)
    {
      return utcDateTime.HasValue ? TimeZoneInfo.ConvertTimeFromUtc(utcDateTime.Value, TZConvert.GetTimeZoneInfo(timezoneId)) : null;
    }

    public static string? GetLocalTimeText(DateTime? utcDateTime, string timezoneId, string format = Constants.Common.DATETIME_FORMAT)
    {
      return GetLocalTime(utcDateTime, timezoneId)?.ToString(format);
    }
  }
}

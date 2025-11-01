using TimeZoneConverter;

namespace ExpressedRealms.Shared;

public static class CommonExtensions
{
    public static DateTimeOffset ToUtc(this DateOnly date, string timeZoneId)
    {
        var tz = TZConvert.GetTimeZoneInfo(timeZoneId);
        var local = date.ToDateTime(TimeOnly.MinValue);
        return new DateTimeOffset(TimeZoneInfo.ConvertTimeToUtc(local, tz), TimeSpan.Zero);
    }

}
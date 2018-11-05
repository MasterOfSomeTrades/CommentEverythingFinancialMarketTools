using System;

namespace CommentEverythingMarketToolsNETCore {
    public static class Clock {
        public static bool IsNowInTimeRange(int startHour, int startMinute, int startSecond, int endHour, int endMinute, int endSecond) {
            bool ret = false;

            TimeSpan start = new TimeSpan(startHour, startMinute, startSecond); // --- 10, 0, 0 = 10 o'clock
            TimeSpan end = new TimeSpan(endHour, endMinute, endSecond); // --- 12, 0, 0 = 12 o'clock

            DateTime timeUtc = DateTime.UtcNow;
            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime easternTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone);
            TimeSpan nowInEasternTime = easternTime.TimeOfDay;

            if ((nowInEasternTime > start) && (nowInEasternTime < end)) {
                ret = true;
            } else {
                ret = false;
            }

            return ret;
        }

        public static bool IsNowInTimeRange(TimeSpan startTime, TimeSpan endTime) {
            bool ret = false;

            DateTime timeUtc = DateTime.UtcNow;
            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime easternTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone);
            TimeSpan nowInEasternTime = easternTime.TimeOfDay;

            if ((nowInEasternTime > startTime) && (nowInEasternTime < endTime)) {
                ret = true;
            } else {
                ret = false;
            }

            return ret;
        }

        public static class TimeZoneId {
            public const string PACIFIC_STANDARD_TIME = "Pacific Standard Time";
            public const string CENTRAL_STANDARD_TIME = "Central Standard Time";
            public const string EASTERN_STANDARD_TIME = "Eastern Standard Time";
            public const string UTC = "UTC";
            public const string GREENWICH_MERIDIAN_TIME = "GMT";
        }

        public static DateTime Convert(DateTime utcTime, string timeZone) {
            TimeZoneInfo convertToTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZone);

            return TimeZoneInfo.ConvertTimeFromUtc(utcTime, convertToTimeZone);
        }

        public static int ConvertToUnixTimestamp(DateTime time, string fromTimeZone) {
            DateTime utcDateTime = TimeZoneInfo.ConvertTimeToUtc(time, TimeZoneInfo.FindSystemTimeZoneById(fromTimeZone));
            return (int) utcDateTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }

        public static DateTime Convert(double unixTimestamp, string timeZone) {
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimestamp);
            TimeZoneInfo convertToTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZone);

            return TimeZoneInfo.ConvertTimeFromUtc(dtDateTime, convertToTimeZone);
        }

        public static DateTime GetCurrentTimeByTimeZone(string timeZone) {
            DateTime timeUtc = DateTime.UtcNow;
            TimeZoneInfo convertToTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZone);

            return TimeZoneInfo.ConvertTimeFromUtc(timeUtc, convertToTimeZone);
        }

        public static string DisplayMonthDayTimeString(DateTime time) {
            return time.ToString("MMMdd HH:mm");
        }

        public static string DisplayDateString(DateTime time) {
            return time.ToString("MMM dd, yyyy");
        }
    }
}

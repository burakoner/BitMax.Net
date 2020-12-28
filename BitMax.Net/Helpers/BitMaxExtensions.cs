using System;

namespace BitMax.Net.Helpers
{
    public static class BitMaxExtensions
    {
        public static DateTime FromUnixTimeSeconds(this int unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }

        public static DateTime FromUnixTimeSeconds(this long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }

        public static long ToUnixTimeSeconds(this DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date - epoch).TotalSeconds);
        }
        
        public static DateTime FromUnixTimeMilliseconds(this long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddMilliseconds(unixTime);
        }

        public static long ToUnixTimeMilliseconds(this DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date - epoch).TotalMilliseconds);
        }
        
        public static bool IsOneOf(this int @this, params int[] values)
        {
            return Array.IndexOf(values, @this) != -1;
        }

        public static bool IsOneOf(this string @this, params string[] values)
        {
            return Array.IndexOf(values, @this) != -1;
        }

        public static bool IsOneOf(this decimal @this, params decimal[] values)
        {
            return Array.IndexOf(values, @this) != -1;
        }

        public static void ValidateStringLength(this string @this, string argumentName, int minLength, int maxLength, string messagePrefix = "", string messageSuffix = "")
        {
            if (@this.Length < minLength || @this.Length > maxLength)
                throw new ArgumentException(
                    $"{messagePrefix}{(messagePrefix.Length > 0 ? " " : "")}{@this} not allowed for parameter {argumentName}, Min Length: {minLength}, Max Length: {maxLength}{(messageSuffix.Length > 0 ? " " : "")}{messageSuffix}");
        }
    }
}

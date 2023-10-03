namespace bs.Frmwrk.Shared
{
    public static class DateTimeExtension
    {
        public static DateTime? ToUtc(this DateTime? value)
        {
            if (value == null) return null;
            return ((DateTime)value).ToUtc();
        }

        public static DateTime ToUtc(this DateTime value)
        {
            if (value.Kind == DateTimeKind.Unspecified)
                return DateTime.SpecifyKind(value, DateTimeKind.Utc);
            else return value;
        }

        public static DateTime? ToDate(this string? value, string format = "yyyy-MM-dd")
        {
            if (value == null || string.IsNullOrWhiteSpace(value)) return null;
            DateTime result = DateTime.ParseExact(value, format, System.Globalization.CultureInfo.InvariantCulture);
            return result;
        }
    }
}
namespace bs.Frmwrk.Shared
{
    public static class StringExtensions
    {
        public static int ToInt(this string? val)
        {
            if (val is null) return 0;
            if (int.TryParse(val, out int result)) return result;
            return 0;
        }

        public static int? ToIntN(this string? val)
        {
            if (val is null) return null;
            if (int.TryParse(val, out int result)) return result;
            return null;
        }

        public static decimal ToDecimal(this string? val)
        {
            if (val is null) return 0;
            if (decimal.TryParse(val, out decimal result)) return result;
            return 0;
        }

        public static decimal? ToDecimalN(this string? val)
        {
            if (val is null) return null;
            if (decimal.TryParse(val, out decimal result)) return result;
            return null;
        }

        public static double ToDouble(this string? val)
        {
            if (val is null) return 0;
            if (double.TryParse(val, out double result)) return result;
            return 0;
        }

        public static double? ToDoubleN(this string? val)
        {
            if (val is null) return null;
            if (double.TryParse(val, out double result)) return result;
            return null;
        }
    }
}
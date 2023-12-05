using bs.Frmwrk.Core.Exceptions;

namespace bs.Frmwrk.Shared
{
    public static class StringExtensions
    {
        /// <summary>
        /// Convert the base64 string to a byte array. If the value is null it returns null.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static byte[]? FromBase64ToByteArray(this string? value)
        {
            if (value == null) return null;
            return Convert.FromBase64String(value);
        }

        /// <summary>
        /// Converts to decimal. (if value is null or value cannnot be converted it returns 0)
        /// </summary>
        /// <param name="val">The value.</param>
        /// <returns></returns>
        public static decimal ToDecimal(this string? val)
        {
            if (val is null) return 0;
            if (decimal.TryParse(val, out decimal result)) return result;
            return 0;
        }

        /// <summary>
        /// Converts to nullable decimal. (if value is null or value cannnot be converted it returns null)
        /// </summary>
        /// <param name="val">The value.</param>
        /// <returns></returns>
        public static decimal? ToDecimalN(this string? val)
        {
            if (val is null) return null;
            if (decimal.TryParse(val, out decimal result)) return result;
            return null;
        }

        public static decimal? ToDecimalStright(this string? val)
        {
            if (val is null) return null;
            if (decimal.TryParse(val, out decimal result)) return result;
            throw new BsException(2311131140, $"Cannot convert to decimal value the string {val}");
        }

        /// <summary>
        /// Converts to double. (if value is null or value cannnot be converted it returns 0)
        /// </summary>
        /// <param name="val">The value.</param>
        /// <returns></returns>
        public static double ToDouble(this string? val)
        {
            if (val is null) return 0;
            if (double.TryParse(val, out double result)) return result;
            return 0;
        }

        /// <summary>
        /// Converts to nullable double. (if value is null or value cannnot be converted it returns null)
        /// </summary>
        /// <param name="val">The value.</param>
        /// <returns></returns>
        public static double? ToDoubleN(this string? val)
        {
            if (val is null) return null;
            if (double.TryParse(val, out double result)) return result;
            return null;
        }

        /// <summary>
        /// Converts the string representation of a number to an integer. (if value is null or value cannnot be converted it returns 0)
        /// </summary>
        /// <param name="val">The value.</param>
        /// <returns></returns>
        public static int ToInt(this string? val)
        {
            if (val is null) return 0;
            if (int.TryParse(val, out int result)) return result;
            return 0;
        }

        /// <summary>
        /// Converts to nullable int. (if value is null or value cannnot be converted it returns null)
        /// </summary>
        /// <param name="val">The value.</param>
        /// <returns></returns>
        public static int? ToIntN(this string? val)
        {
            if (val is null) return null;
            if (int.TryParse(val, out int result)) return result;
            return null;
        }

        /// <summary>
        /// Converts the string representation of a number to an integer. (if value is null or value cannnot be converted it returns 0)
        /// </summary>
        /// <param name="val">The value.</param>
        /// <returns></returns>
        public static long ToLong(this string? val)
        {
            if (val is null) return 0;
            if (long.TryParse(val, out long result)) return result;
            return 0;
        }

        /// <summary>
        /// Converts to nullable int. (if value is null or value cannnot be converted it returns null)
        /// </summary>
        /// <param name="val">The value.</param>
        /// <returns></returns>
        public static long? ToLongN(this string? val)
        {
            if (val is null) return null;
            if (long.TryParse(val, out long result)) return result;
            return null;
        }
    }
}
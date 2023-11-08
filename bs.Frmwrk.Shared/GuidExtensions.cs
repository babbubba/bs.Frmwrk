using bs.Frmwrk.Core.Exceptions;

namespace bs.Frmwrk.Shared
{
    public static class GuidExtensions
    {
        public static Guid? ToGuidN(this string? val)
        {
            if (val is null) return null;
            if (Guid.TryParse(val, out Guid result)) return result;
            return null;
        }

        public static Guid ToGuid(this string? val)
        {
            if (val is null) return Guid.Empty;

            try
            {
                return Guid.Parse(val);
            }
            catch (Exception)
            {
                throw new BsException(2311081207, $"Invalid GUID ({val})");
            }
        }

        public static Guid[] ToGuid(this string[]? val)
        {
            if (val is null) return Array.Empty<Guid>();

            try
            {
                return val.Select(v => v.ToGuid()).ToArray();
            }
            catch (Exception)
            {
                throw new BsException(2311081208, $"Invalid GUID in collection ({val})");
            }
        }

        public static bool IsValidGuid(this string stringValue)
        {
            var guidValue = stringValue.ToGuidN();

            return guidValue is not null;
        }

        public static bool IsValidGuid(this string stringValue, ref Guid returnValue)
        {
            var guidValue = stringValue.ToGuidN();
            if (guidValue == null) return false;
            returnValue = (Guid)guidValue;
            return true;
        }
    }
}
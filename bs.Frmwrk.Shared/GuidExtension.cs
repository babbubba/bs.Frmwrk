namespace bs.Frmwrk.Shared
{
    public static class GuidExtension
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
                throw new ApplicationException("Invalid GUID.");
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
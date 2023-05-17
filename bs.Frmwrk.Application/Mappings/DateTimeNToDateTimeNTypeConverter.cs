using AutoMapper;
using bs.Frmwrk.Shared;

namespace bs.Frmwrk.Application.Mappings {
    public class DateTimeNToDateTimeNTypeConverter : ITypeConverter<DateTime?, DateTime?>
    {
        public DateTime? Convert(DateTime? source, DateTime? destination, ResolutionContext context)
        {
            return source.ToUtc();
        }
    }
}
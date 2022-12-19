using bs.Frmwrk.Mapper.Profiles;

namespace bs.Frmwrk.Application.Mappings
{
    public class MappingProfileBase : MappingProfile
    {
        public MappingProfileBase()
        {
            // Add the utc kind if not defined in source data
            CreateMap<DateTime, DateTime>()
                .ConvertUsing<DateTimeToDateTimeTypeConverter>();
            CreateMap<DateTime?, DateTime?>()
                .ConvertUsing<DateTimeNToDateTimeNTypeConverter>();
        }
    }
}
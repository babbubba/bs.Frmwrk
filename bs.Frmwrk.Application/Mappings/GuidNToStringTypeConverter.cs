using AutoMapper;

namespace bs.Frmwrk.Application.Mappings {
    public class GuidNToStringTypeConverter : ITypeConverter<Guid?, string?> {
        public string? Convert(Guid? source, string? destination, ResolutionContext context) {
            return (source != null) ? source.ToString() : null;
        }
    }
}
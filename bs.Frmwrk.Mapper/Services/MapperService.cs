using AutoMapper;
using bs.Frmwrk.Core.Mapper.Profiles;
using bs.Frmwrk.Core.Services.Mapping;

namespace bs.Frmwrk.Mapper.Services
{
    public class MapperService : IMapperService
    {
        private readonly IMapper mapper;

        public MapperService(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public TDestination Map<TDestination>(object source)
        {
            return mapper.Map<TDestination>(source);
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return mapper.Map<TSource, TDestination>(source);
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            return mapper.Map(source, destination);
        }

        public object Map(object source, Type sourceType, Type destinationType)
        {
            return mapper.Map(source, sourceType, destinationType);
        }

        public object Map(object source, object destination, Type sourceType, Type destinationType)
        {
            return mapper.Map(source, destination, sourceType, destinationType);
        }
    }
}
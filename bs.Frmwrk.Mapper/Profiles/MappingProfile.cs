using AutoMapper;
using bs.Frmwrk.Core.Mapper.Profiles;

namespace bs.Frmwrk.Mapper.Profiles
{
    public abstract class MappingProfile : Profile, IMappingProfile
    {
        /// <summary>
        /// Creates the mapping.
        /// </summary>
        /// <typeparam name="TSourceInterface">The type of the source interface.</typeparam>
        /// <typeparam name="TDestinationInterface">The type of the destination interface.</typeparam>
        /// <typeparam name="TDestinationClass">The type of the destination class (concrete type) that implements destination interface.</typeparam>
        public void CreateMapping<TSourceInterface, TDestinationInterface, TDestinationClass>() where TDestinationClass : TDestinationInterface
        {
            CreateMap<TSourceInterface, TDestinationInterface>().As<TDestinationClass>();
            CreateMap<TSourceInterface, TDestinationClass>();
        }

        public void CreateMappingWithReverse<TSourceInterface, TDestinationInterface, TSourceClass, TDestinationClass>() where TDestinationClass : TDestinationInterface where TSourceClass : TSourceInterface
        {
            CreateMap<TSourceInterface, TDestinationInterface>().As<TDestinationClass>();
            CreateMap<TSourceInterface, TDestinationClass>();

            CreateMap<TDestinationInterface, TSourceInterface>().As<TSourceClass>();
            CreateMap<TDestinationInterface, TSourceClass>();
        }
    }
}
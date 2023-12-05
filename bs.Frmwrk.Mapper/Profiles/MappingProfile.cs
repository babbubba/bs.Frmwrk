using AutoMapper;
using bs.Frmwrk.Core.Mapper.Profiles;
using bs.Frmwrk.Core.Models.Base;
using bs.Frmwrk.Shared;

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
        public void CreateMapping<TSourceInterface, TDestinationInterface, TDestinationClass>(bool ignoreId = false) where TDestinationClass : TDestinationInterface, IIdentified
        {
            CreateMap<TSourceInterface, TDestinationInterface>().As<TDestinationClass>();
            IMappingExpression<TSourceInterface, TDestinationClass> exp1 = CreateMap<TSourceInterface, TDestinationClass>();
            if (ignoreId)
            {
                exp1.ForMember(m => m.Id, opt => opt.Ignore());
            }
        }

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

        public void CreateMapping<TSourceInterface, TDestinationInterface>()
        {
            IMappingExpression<TSourceInterface, TDestinationInterface> baseMapping = CreateMap<TSourceInterface, TDestinationInterface>();
            
            var concreteClass = typeof(TDestinationInterface).GetImplTypeFromInterface();
            if (concreteClass is not null)
            {
                baseMapping.As(concreteClass);
                CreateMap(typeof(TSourceInterface), concreteClass);
            }
        }
    }
}
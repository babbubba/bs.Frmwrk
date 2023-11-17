using bs.Frmwrk.Core.Models.Base;

namespace bs.Frmwrk.Core.Mapper.Profiles
{
    /// <summary>
    ///
    /// </summary>
    public interface IMappingProfile
    {
        /// <summary>
        /// Creates the mapping.
        /// </summary>
        /// <typeparam name="TSourceInterface">The type of the source interface.</typeparam>
        /// <typeparam name="TDestinationInterface">The type of the destination interface.</typeparam>
        /// <typeparam name="TDestinationClass">The type of the destination class (concrete type) that implements destination interface.</typeparam>
        void CreateMapping<TSourceInterface, TDestinationInterface, TDestinationClass>() where TDestinationClass : TDestinationInterface;

        /// <summary>
        /// Creates the mapping.
        /// </summary>
        /// <typeparam name="TSourceInterface">The type of the source interface.</typeparam>
        /// <typeparam name="TDestinationInterface">The type of the destination interface.</typeparam>
        /// <typeparam name="TDestinationClass">The type of the destination class.</typeparam>
        /// <param name="ignoreId">if set to <c>true</c> [ignore identifier].</param>
        void CreateMapping<TSourceInterface, TDestinationInterface, TDestinationClass>(bool ignoreId = false) where TDestinationClass : TDestinationInterface, IIdentified;

        /// <summary>
        /// Creates the mapping from source interface to targe interface and derived class (using reflection).
        /// </summary>
        /// <typeparam name="TSourceInterface">The type of the source interface.</typeparam>
        /// <typeparam name="TDestinationInterface">The type of the destination interface.</typeparam>
        void CreateMapping<TSourceInterface, TDestinationInterface>();
    }
}
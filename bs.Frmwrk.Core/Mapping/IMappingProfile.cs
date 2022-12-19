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
    }
}
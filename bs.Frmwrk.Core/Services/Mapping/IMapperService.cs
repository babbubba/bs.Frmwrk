namespace bs.Frmwrk.Core.Services.Mapping
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMapperService
    {
        /// <summary>
        /// Maps the type to the output type
        /// </summary>
        /// <typeparam name="TDestination">The type of the destination.</typeparam>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        TDestination Map<TDestination>(object source);
        /// <summary>
        /// Maps the specified source.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TDestination">The type of the destination.</typeparam>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        TDestination Map<TSource, TDestination>(TSource source);
        /// <summary>
        /// Maps the specified source.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TDestination">The type of the destination.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        /// <returns></returns>
        TDestination Map<TSource, TDestination>(TSource source, TDestination destination);
        /// <summary>
        /// Maps the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="sourceType">Type of the source.</param>
        /// <param name="destinationType">Type of the destination.</param>
        /// <returns></returns>
        object Map(object source, Type sourceType, Type destinationType);
        /// <summary>
        /// Maps the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        /// <param name="sourceType">Type of the source.</param>
        /// <param name="destinationType">Type of the destination.</param>
        /// <returns></returns>
        object Map(object source, object destination, Type sourceType, Type destinationType);
    }
}
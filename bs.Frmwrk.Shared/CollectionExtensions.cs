using bs.Frmwrk.Core.Exceptions;

namespace bs.Frmwrk.Shared
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// Adds if not exists. It comp
        /// </summary>
        /// <typeparam name="T">The type of the item of the collection</typeparam>
        /// <typeparam name="R">The type of the property to compare</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="itemToAdd">The item to add if not exists in the collection yet.</param>
        /// <param name="parameter">The property used to compare the collection with the item to add.</param>
        /// <exception cref="bs.Frmwrk.Core.Exceptions.BsException">2212211504 - Parameter is mandatory</exception>
        public static void AddIfNotExists<T, R>(this ICollection<T> collection, T itemToAdd, Func<T, R> parameter)
        {
            if (parameter is null)
            {
                throw new BsException(2212211504, "Parameter is mandatory");
            }

#pragma warning disable CS8602 // Dereferenziamento di un possibile riferimento Null.
            if (!collection.Any(v => parameter(v).Equals(parameter(itemToAdd))))
            {
                collection.Add(itemToAdd);
            }
#pragma warning restore CS8602 // Dereferenziamento di un possibile riferimento Null.
        }
    }
}
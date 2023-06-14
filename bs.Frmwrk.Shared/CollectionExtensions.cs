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
        /// <param name="matchingField">The property used to compare the collection with the item to add.</param>
        /// <exception cref="bs.Frmwrk.Core.Exceptions.BsException">2212211504 - Parameter is mandatory</exception>
        public static bool AddIfNotExists<T, R>(this ICollection<T>? collection, T itemToAdd, Func<T, R> matchingField)
        {
            if (matchingField == null)
            {
                throw new BsException(2212211504, "Parameter is mandatory");
            }

            collection ??= new List<T>();

            if (!collection.Any(v => matchingField(v).Equals(matchingField(itemToAdd))))
            {
                collection.Add(itemToAdd);
                return true;
            }
            return false;
        }

        public static void UpdateLists<ListType, MatchValueType>(this IList<ListType> outputList, IList<ListType> inputList, Func<ListType, MatchValueType> matchingField) where ListType : class
        {
            if (outputList is null)
            {
                throw new BsException(2305181635, "Output list is null, cannot update the list");
            }

            foreach (var inputElement in inputList)
            {
                if (!outputList.Any(g => matchingField(g).Equals(matchingField(inputElement))))
                {
                    // if the output list not contains this item we have to add it to output
                    outputList.Add(inputElement);
                }
            }

            for (var idx = 0; idx < outputList.Count();)
            {
                if (!inputList.Any(g => matchingField(g).Equals(matchingField(outputList[idx]))))
                {
                    // if the input list not contains this item we have to remove it from output list
                    outputList.Remove(outputList[idx]);
                }
                else
                {
                    // increment the index only id item was not removed
                    idx++;
                }
            }
        }

        public static void UpdateLists<ListType>(this IList<ListType> outputList, IList<ListType> inputList) where ListType : class
        {
            if (outputList is null)
            {
                throw new BsException(2305181636, "Output list is null, cannot update the list");
            }

            foreach (var inputElement in inputList)
            {
                if (!outputList.Any(g => g.Equals(inputElement)))
                {
                    // if the output list not conatins this item we have to add it to output
                    outputList.Add(inputElement);
                }
            }
            for (var idx = 0; idx < outputList.Count();)
            {
                if (!inputList.Any(g => g.Equals(outputList[idx])))
                {
                    // if the input list not contains this item we have to remove it from output list
                    outputList.Remove(outputList[idx]);
                }
                else
                {
                    // increment the index only id item was not removed
                    idx++;
                }
            }
        }
    }
}
using bs.Frmwrk.Core.Exceptions;
using bs.Frmwrk.Core.Models.Base;

namespace bs.Frmwrk.Shared
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// Returns a list with elements contained in first but not in second using the specified field comparartor
        /// </summary>
        /// <typeparam name="FistListType">The type of the source.</typeparam>
        /// <typeparam name="FieldType">The type of the ield type.</typeparam>
        /// <param name="first">The first.</param>
        /// <param name="second">The second.</param>
        /// <param name="matchingField">The matching field.</param>
        /// <returns></returns>
        /// <exception cref="bs.Frmwrk.Core.Exceptions.BsException">
        /// 2312121228 - The source list is mandatory
        /// or
        /// 2312121228 - The list to compare is mandatory
        /// </exception>
        public static IEnumerable<FistListType> Except<FistListType, FieldType>(this IEnumerable<FistListType> first, IEnumerable<FistListType> second, Func<FistListType, FieldType> matchingField)
        {
            if (first == null)
            {
                throw new BsException(2312121228, "The first list is mandatory");
            }

            if (second == null)
            {
                throw new BsException(2312121228, "The second list to compare is mandatory");
            }

            var secondValuesHashSet = new HashSet<FieldType>(second.Select(matchingField));

            return first.Where(item => !secondValuesHashSet.Contains(matchingField(item)));
        }

        /// <summary>
        /// Adds if not exists. It comp
        /// </summary>
        /// <typeparam name="ListType">The type of the item of the collection</typeparam>
        /// <typeparam name="FieldType">The type of the property to compare</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="itemToAdd">The item to add if not exists in the collection yet.</param>
        /// <param name="matchingField">The property used to compare the collection with the item to add.</param>
        /// <exception cref="bs.Frmwrk.Core.Exceptions.BsException">2212211504 - Parameter is mandatory</exception>
        public static bool AddIfNotExists<ListType, FieldType>(this ICollection<ListType>? collection, ListType itemToAdd, Func<ListType, FieldType> matchingField)
        {
            if (matchingField == null)
            {
                throw new BsException(2212211504, "The 'collection' parameter is mandatory");
            }

            if (itemToAdd == null)
            {
                throw new BsException(2312121243, "The 'itemToAdd' parameter is mandatory");
            }

            collection ??= new List<ListType>();

            if (!collection.Any(v => matchingField(v).Equals(matchingField(itemToAdd))))
            {
                collection.Add(itemToAdd);
                return true;
            }
            return false;
        }

        /// <summary>
        /// It updates the first list adding element in the second list not present in the first list and removing element present in first list and not present in the second one.
        /// The comparation is defined by the field in the matchingField parameter.
        /// </summary>
        /// <typeparam name="ListType">The type of the lists elements.</typeparam>
        /// <typeparam name="MatchValueType">The type of the field used to compare elements.</typeparam>
        /// <param name="firstList">The first list that will be updated.</param>
        /// <param name="secondList">The second list to add to the first list.</param>
        /// <param name="matchingField">The matching field.</param>
        /// <exception cref="bs.Frmwrk.Core.Exceptions.BsException">2305181635 - Output list is null, cannot update the list</exception>
        public static void UpdateLists<ListType, MatchValueType>(this ICollection<ListType> firstList, ICollection<ListType> secondList, Func<ListType, MatchValueType> matchingField) where ListType : class
        {
            if (firstList is null)
            {
                throw new BsException(2305181635, "The parameter 'firstList' is mandatory");
            }

            if (secondList is null)
            {
                throw new BsException(2305181635, "The parameter 'secondList' is mandatory");
            }

            // Add elements currently in second list but now in first list
            foreach (var elementToAdd in secondList.Except(firstList, matchingField))
            {
                firstList.Add(elementToAdd);
            }

            // Remove elements in first list that is not present in second list
            var secondListHashSet = new HashSet<MatchValueType>(secondList.Select(matchingField));
            foreach (var elementToRemove in firstList.ToList())
            {
                var firstListMatchingValue = matchingField(elementToRemove);

                if (!secondListHashSet.Contains(firstListMatchingValue))
                {
                    firstList.Remove(elementToRemove);
                }
            }
        }

        /// <summary>
        /// It updates the first list adding element in the second list not present in the first list and removing element present in first list and not present in the second one.
        /// The comparation is defined by the default 'ListType' equality comparer.
        /// </summary>
        /// <typeparam name="ListType">The type of the ist type.</typeparam>
        /// <param name="firstList">The first list.</param>
        /// <param name="secondList">The second list.</param>
        /// <exception cref="bs.Frmwrk.Core.Exceptions.BsException">
        /// 2305181635 - The parameter 'firstList' is mandatory
        /// or
        /// 2305181635 - The parameter 'secondList' is mandatory
        /// </exception>
        public static void UpdateLists<ListType>(this ICollection<ListType> firstList, ICollection<ListType> secondList) where ListType : class
        {
            if (firstList is null)
            {
                throw new BsException(2305181635, "The parameter 'firstList' is mandatory");
            }

            if (secondList is null)
            {
                throw new BsException(2305181635, "The parameter 'secondList' is mandatory");
            }

            secondList.Except(firstList).ToList().ForEach(elementsToAdd => firstList.Add(elementsToAdd));

            foreach (var element in firstList.Except(secondList).ToList())
            {
                firstList.Remove(element);
            }
        }

        /// <summary>
        /// It updates the first list adding element in the second list not present in the first list and removing element present in first list and not present in the second one.
        /// The comparation is defined by the GUID  identifier.
        /// </summary>
        /// <typeparam name="ListType">The type of the ist type.</typeparam>
        /// <param name="firstList">The first list.</param>
        /// <param name="secondList">The second list.</param>
        public static void UpdateListsById<ListType>(this ICollection<ListType> firstList, ICollection<ListType> secondList) where ListType : class, IIdentified
        {
            firstList.UpdateLists(secondList, e => e.Id);
        }
    }
}
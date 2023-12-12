﻿using bs.Frmwrk.Core.Exceptions;
using bs.Frmwrk.Core.Models.Base;

namespace bs.Frmwrk.Shared
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// Returns a list with elements contained in first but not in second using the specified field comparartor
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
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
        public static IEnumerable<TSource> Except<TSource, FieldType>(this IEnumerable<TSource> first, IEnumerable<TSource> second, Func<TSource, FieldType> matchingField)
        {
            if (first == null)
            {
                throw new BsException(2312121228, "The source list is mandatory");
            }

            if (second == null)
            {
                throw new BsException(2312121228, "The list to compare is mandatory");
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
        /// Updates the list with value in second list not present in the list.
        /// </summary>
        /// <typeparam name="ListType">The type of the list type.</typeparam>
        /// <typeparam name="MatchValueType">The type of the match value type.</typeparam>
        /// <param name="outputList">The resultant list.</param>
        /// <param name="inputList">The second list to add to the list.</param>
        /// <param name="matchingField">The matching field.</param>
        /// <exception cref="bs.Frmwrk.Core.Exceptions.BsException">2305181635 - Output list is null, cannot update the list</exception>
        public static void UpdateLists<ListType, MatchValueType>(this IList<ListType> outputList, IList<ListType> inputList, Func<ListType, MatchValueType> matchingField) where ListType : class
        {
            if (outputList is null)
            {
                throw new BsException(2305181635, "The parameter 'outputList' is mandatory");
            }

            var inputSet = new HashSet<MatchValueType>(inputList.Select(matchingField));

            // Aggiungi gli elementi che sono in input ma non in output
            foreach (var inputElement in inputList)
            {
                var inputFieldValue = matchingField(inputElement);
                if (!inputSet.Contains(inputFieldValue))
                {
                    outputList.Add(inputElement);
                    inputSet.Add(inputFieldValue);  // Aggiungi il valore al set per evitare ricerche inutili successivamente
                }
            }

            // Rimuovi gli elementi che sono in output ma non in input
            for (var idx = outputList.Count - 1; idx >= 0; idx--)
            {
                var outputFieldValue = matchingField(outputList[idx]);
                if (!inputSet.Contains(outputFieldValue))
                {
                    outputList.RemoveAt(idx);
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

        public static void UpdateListsById<ListType>(this IList<ListType> outputList, IList<ListType> inputList) where ListType : IIdentified
        {
            if (outputList is null)
            {
                throw new BsException(2305181635, "Output list is null, cannot update the list");
            }

            foreach (var inputElement in inputList)
            {
                if (!outputList.Any(g => g.Id == inputElement.Id))
                {
                    // if the output list not contains this item we have to add it to output
                    outputList.Add(inputElement);
                }
            }

            for (var idx = 0; idx < outputList.Count();)
            {
                if (!inputList.Any(g => g.Id == outputList[idx].Id))
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
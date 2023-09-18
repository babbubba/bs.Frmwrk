namespace bs.Frmwrk.Shared
{
    public static class HierarchyExtensions
    {
        /// <summary>
        /// Gets the childrens hierarchy in multilevel hierarchy. It returns the elements from the ancestor to the last child.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent">The parent.</param>
        /// <param name="childrenSelector">The children selector.</param>
        /// <param name="result">The result.</param>
        public static void GetChildrensHierarchy<T>(this T parent, Func<T, IEnumerable<T>> childrenSelector, List<T> result)
        {
            if (childrenSelector(parent) != null && childrenSelector(parent).Any())
            {
                foreach (var child in childrenSelector(parent))
                {
                    result.Add(child);
                    child.GetChildrensHierarchy(childrenSelector, result);
                }
            }
        }

        /// <summary>
        /// Gets the parents hierarchy in a multilevel hierarchy. It return the elements from the child to the ancestor parent (the parent without parent)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="child">The lowest level in hierarchy (the child).</param>
        /// <param name="parentSelector">The selector (the property of element that references to parent element).</param>
        /// <example>
        /// this.GetParentsHierarchy-IStructure-(x => x.Parent) //return an ienumerable of element from child to parent.
        /// </example>
        /// <returns></returns>
        public static IEnumerable<T> GetParentsHierarchy<T>(this T child, Func<T, T> parentSelector)
        {
            var currentObject = child;
            while (parentSelector(currentObject) != null)
            {
                yield return currentObject;
                currentObject = parentSelector(currentObject);
            }

            //this is the ancestor parent
            yield return currentObject;
        }
    }
}
using System.Linq.Expressions;

namespace bs.Frmwrk.Shared
{
    /// <summary>
    /// Linq  Extensions
    /// </summary>
    public static class LinqExtensions
    {
        /// <summary>
        /// Dynamically order by Quryable object. It order the queryable object by the property provided in string format using reflection
        /// </summary>
        /// <typeparam name="TEntity">The type of the Queryable entity.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="orderByProperty">The property to order by.</param>
        /// <param name="desc">If set to <c>true</c> it will order descending otherwise ascending.</param>
        /// <returns></returns>
        /// <exception cref="System.ApplicationException">Invalid property name '{orderByProperty}' for object: '{typeof(TEntity).Name}'</exception>
        public static IQueryable<TEntity> DynamicOrderBy<TEntity>(this IQueryable<TEntity> source, string orderByProperty,
                          bool desc)
        {
            string command = desc ? "OrderByDescending" : "OrderBy";
            var type = typeof(TEntity);
            var property = type.GetProperty(orderByProperty);
            if (property == null)
            {
                throw new ApplicationException($"Invalid property name '{orderByProperty}' for object: '{typeof(TEntity).Name}'");
            }
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { type, property.PropertyType },
                                          source.Expression, Expression.Quote(orderByExpression));
            return source.Provider.CreateQuery<TEntity>(resultExpression);
        }

        /// <summary>
        ///  Dynamically order by Queryable object. It order the queryable object by the property provided in string format using reflection.
        ///  If you need a nested propertiy you have to provide the orderByPropery parameter hierarchy dot separated.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="orderByProperty">The property to order by. If you want a nested property simly use the 'dot' (for ex: 'parentProperty.childProperty'). </param>
        /// <param name="desc">If set to <c>true</c> it will order descending otherwise ascending.</param>
        /// <returns></returns>
        /// <exception cref="System.ApplicationException">
        /// Invalid property name '{orderByProperty}' for object: '{typeof(TEntity).Name}'
        /// or
        /// Invalid property name '{propertiesTree[0]}' for object: '{typeof(TEntity).Name}'
        /// </exception>
        public static IQueryable<TEntity> DynamicOrderNestedBy<TEntity>(this IQueryable<TEntity> source, string orderByProperty,
                          bool desc)
        {
            string command = desc ? "OrderByDescending" : "OrderBy";
            var propertiesTree = orderByProperty.Split('.');
            MethodCallExpression? resultExpression = null;

            if (propertiesTree.Length == 1)
            {
                var type = typeof(TEntity);
                var property = type.GetProperty(orderByProperty);
                if (property == null)
                {
                    throw new ApplicationException($"Invalid property name '{orderByProperty}' for object: '{typeof(TEntity).Name}'");
                }
                var parameter = Expression.Parameter(type, "p");
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExpression = Expression.Lambda(propertyAccess, parameter);
                resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { type, property.PropertyType },
                                              source.Expression, Expression.Quote(orderByExpression));
            }
            if (propertiesTree.Length == 2)
            {
                var parentType = typeof(TEntity);
                var partentProperty = parentType.GetProperty(propertiesTree[0]);
                if (partentProperty == null)
                {
                    throw new ApplicationException($"Invalid property name '{propertiesTree[0]}' for object: '{typeof(TEntity).Name}'");
                }

                var type = partentProperty.PropertyType;
                var property = type.GetProperty(propertiesTree[1]);
                if (property == null)
                {
                    throw new ApplicationException($"Invalid property name '{orderByProperty}' for object: '{typeof(TEntity).Name}'");
                }
                var orderByExpression = CreateExpression(parentType, orderByProperty);
                resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { parentType, property.PropertyType },
                                            source.Expression, Expression.Quote(orderByExpression));
            }

            if (resultExpression == null) return source;

            return source.Provider.CreateQuery<TEntity>(resultExpression);
        }

        /// <summary>
        /// Gets the member expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aliasExpression">The alias expression.</param>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        private static Expression<Func<object>> GetMemberExpression<T>(Expression<Func<object>> aliasExpression, string property)
        {
            var propertyExpression = Expression.Property(aliasExpression.Body,
                                                         typeof(T), property);
            var body = Expression.Convert(propertyExpression, typeof(object));

            var result = Expression.Lambda<Func<object>>(body);
            return result;
        }

        /// <summary>
        /// Creates the expression.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        private static LambdaExpression CreateExpression(Type type, string propertyName)
        {
            var param = Expression.Parameter(type, "x");
            Expression body = param;
            foreach (var member in propertyName.Split('.'))
            {
                body = Expression.PropertyOrField(body, member);
            }
            return Expression.Lambda(body, param);
        }
    }
}
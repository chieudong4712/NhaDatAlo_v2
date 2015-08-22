using ResourceMetadata.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ResourceMetadata.Core.Util
{
    
    public static class LINQExtensions
    {
        #region Order
        public static IOrderedEnumerable<T> OrderByPropertyName<T>(this IEnumerable<T> source, string propertyName, SortType orderDirection = SortType.ASC) where T:class
        {
            MemberInfo member = typeof(T).GetProperty(propertyName);
            var x = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(x, propertyName);
            var conversion = Expression.Convert(property, typeof (object));
            var lambda = Expression.Lambda<Func<T, dynamic>>(conversion, x).Compile();

            IOrderedEnumerable<T> orderedSource;
            if (orderDirection == SortType.ASC)
            {
                orderedSource = source.OrderBy(lambda);
            }
            else
	        {
                orderedSource = source.OrderByDescending(lambda);
	        }
            return orderedSource;
        }

        public static IOrderedEnumerable<T> OrderByPropertyName<T>(this IEnumerable<T> source, string propertyName, string orderDirection) where T : class
        {
            var sortType = String.IsNullOrEmpty(orderDirection) ? SortType.ASC : ConvertUtil.ToEnum<SortType>(orderDirection);
            return source.OrderByPropertyName(propertyName, sortType);
        }

        public static IOrderedQueryable<T> OrderByPropertyName<T>(this IQueryable<T> source, string propertyName, string orderDirection)
        {
            var sortType = String.IsNullOrEmpty(orderDirection) ? SortType.ASC : ConvertUtil.ToEnum<SortType>(orderDirection);
            return source.OrderByPropertyName(propertyName, sortType);
        }
        
        public static IOrderedQueryable<T> OrderByPropertyName<T>(this IQueryable<T> source, string propertyName, SortType orderDirection = SortType.ASC)
        {
            MemberInfo member = typeof(T).GetProperty(propertyName);
            var x = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(x, propertyName);
            var lambda = Expression.Lambda(property, x);

            bool descending = orderDirection == SortType.DESC;

            MethodCallExpression call = Expression.Call(
                typeof(Queryable),
                "OrderBy" + (descending ? "Descending" : string.Empty),
                new[] { typeof(T), property.Type },
                source.Expression,
                Expression.Quote(lambda));

            return (IOrderedQueryable<T>)source.Provider.CreateQuery<T>(call);
        }

        #endregion

        #region WHERE

        /// <summary>
        /// Dynamic Where clause, BinaryExpressionName parameter is name of BinaryExpression class example: Equal, NotEqual, GreaterThan...
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <param name="binaryExpressionName"></param>
        /// <param name="targetValue"></param>
        /// <returns></returns>
        public static IQueryable<T> WhereByCustom<T>(this IQueryable<T> source, string propertyName, BinaryExpressionName binaryExpressionName, string targetValue) where T : class 
        {
            
            ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
            Expression property = Expression.Property(parameter, propertyName);
            Expression target = Expression.Constant(targetValue);

            var op = typeof(Expression).GetMethod(binaryExpressionName.ToString(), BindingFlags.Public | BindingFlags.Static,
            null, new[] { typeof(Expression), typeof(Expression) }, null);

            var exp = (Expression)op.Invoke(null, new object[] { property, target });

            var lambda = Expression.Lambda<Func<T, bool>>(exp, parameter);

            return source.Where(lambda);
        }
        #endregion
    }
}
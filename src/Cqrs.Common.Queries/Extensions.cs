namespace Cqrs.Common.Queries
{
    #region
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    #endregion

    public static class Extensions
    {
        public static string ToPropertyName<T, TResult>(this Expression<Func<T, TResult>> selector)
        {
            var me = selector.Body as MemberExpression;
            if (me == null)
            {
                throw new ArgumentException("MemberException expected.");
            }

            var propertyName = me.ToString().Remove(0, 2);
            return propertyName;
        }

        public static IEnumerable<string> GetProperiesNames(this Type type)
        {
            return type.GetTypeInfo().DeclaredProperties.Select(p => p.Name);
        }
    }
}
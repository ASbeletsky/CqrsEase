namespace Cqrs.Common.Queries
{
    using System;
    using System.Linq.Expressions;

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
    }
}
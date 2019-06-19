namespace CqrsEase.Common.Queries
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
            var me = selector.Body.AsMemberExpression() ?? throw new ArgumentException("Member expresison expected");
            var propertyName = me.ToString().Remove(0, 2);
            return propertyName;
        }

        public static IEnumerable<string> GetProperiesNames(this Type type)
        {
            return type.GetTypeInfo().DeclaredProperties.Select(p => p.Name);
        }

        internal static MemberExpression AsMemberExpression(this Expression exp)
        {
            switch (exp)
            {
                case MemberExpression me: return me;
                case UnaryExpression unaryExpression: return unaryExpression.Operand as MemberExpression;
                default: throw new ArgumentException("Cannot convert to member expresison.");
            }
        }
    }
}
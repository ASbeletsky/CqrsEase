using NSpecifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace CqrsEase.Web.JsonApi
{
    internal static class SpecificationFactory<T> 
    {
        internal static ASpec<T> CreateEqualitySpecification(string memberName, object memberValue)
        {
            if (memberName == null)
                throw new ArgumentNullException("memberName");

            if(memberValue == null)
                throw new ArgumentNullException("memberValue");

            var memberType = typeof(T).GetTypeInfo().GetDeclaredProperty(memberName)?.PropertyType;
            if(memberType == null)
                throw new InvalidOperationException($"Type {typeof(T).Name} doesn't contain member {memberName}");
 
            var parameter = Expression.Parameter(typeof(T), "x");
            var fieldExp = Expression.Property(parameter, memberName);
            var fieldValueExp = Expression.Constant(memberValue, memberType);
            var equalExp = Expression.Equal(fieldExp, fieldValueExp);
            var lamda = Expression.Lambda(equalExp, parameter);
            var spec = (Spec<T>)Activator.CreateInstance(typeof(Spec<T>), lamda);
            return spec;
        }

        internal static ASpec<T> Сonjunction(IEnumerable<ASpec<T>> specs)
        {
            return specs.Any() ? specs.Aggregate((curr, next) => curr & next) : null;
        }
    }
}

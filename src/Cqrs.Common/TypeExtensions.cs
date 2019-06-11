using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cqrs.Common
{
    public static class TypeExtensions
    {
        public static bool IsEnumerableOf<T>(this Type type, out Type itemType, bool excludeString = true)
        {
            itemType = null;
            if (excludeString && type == typeof(string)) return false;

            var typeInfo = type.GetTypeInfo();
            var itemTypeInfo = typeof(T).GetTypeInfo();
            if (type.IsConstructedGenericType)
            {
                
                if (type.IsTypeDefinitionEnumerable() ||
                    typeInfo.ImplementedInterfaces.Any(t => t.IsSelfEnumerable() || t.IsTypeDefinitionEnumerable()))
                {
                    itemType = typeInfo.GenericTypeArguments[0];
                    return itemTypeInfo.IsAssignableFrom(itemType.GetTypeInfo());
                }
            }
            //direct implementations of IEnumerable<T>, inheritance from List<T> etc
            var enumerableOrNull = typeInfo.ImplementedInterfaces.FirstOrDefault(t => t.IsTypeDefinitionEnumerable());
            if (enumerableOrNull == null) return false;

            itemType = enumerableOrNull.GetTypeInfo().GenericTypeArguments[0];
            return itemTypeInfo.IsAssignableFrom(itemType.GetTypeInfo());
        }

        private static bool IsSelfEnumerable(this Type type)
        {
            bool isDirectly = type == typeof(IEnumerable<>);
            return isDirectly;
        }

        private static bool IsTypeDefinitionEnumerable(this Type type)
        {
            bool isViaInterfaces = type.IsConstructedGenericType &&
                                   type.GetGenericTypeDefinition().IsSelfEnumerable();
            return isViaInterfaces;
        }
    }
}

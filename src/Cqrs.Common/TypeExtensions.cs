namespace Cqrs.Common
{
    #region Using
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    #endregion

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

        public static PropertyInfo GetInerfaceProperty(this Type type, string propertyName)
        {
            var typeInfo = type.GetTypeInfo();
            PropertyInfo prop = typeInfo.GetDeclaredProperty(propertyName);
            if (prop == null)
            {
                var baseTypesAndInterfaces = new List<Type>();
                if (typeInfo.BaseType != null) baseTypesAndInterfaces.Add(typeInfo.BaseType);
                baseTypesAndInterfaces.AddRange(type.GetTypeInfo().ImplementedInterfaces);
                foreach (Type t in baseTypesAndInterfaces)
                {
                    prop = GetInerfaceProperty(t, propertyName);
                    if (prop != null)
                        break;
                }
            }

            return prop;
        }

        public static bool IsAnonymous(this Type type)
        {
            var typeInfo = type.GetTypeInfo();
            if (typeInfo.IsGenericType)
            {
                var d = type.GetGenericTypeDefinition().GetTypeInfo();
                if (d.IsClass && d.IsSealed && d.Attributes.HasFlag(TypeAttributes.NotPublic))
                {
                    var attributes = d.GetCustomAttributes(typeof(CompilerGeneratedAttribute), false);
                    return attributes != null && attributes.Any();
                }
            }

            return false;
        }
    }
}

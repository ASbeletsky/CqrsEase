namespace Cqrs.EntityFrameworkCore
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
        public static PropertyInfo GetInerfaceProperty(this Type type, string propertyName)
        {
            var typeInfo = type.GetTypeInfo();
            PropertyInfo prop = typeInfo.GetDeclaredProperty(propertyName);
            if (prop == null)
            {
                var baseTypesAndInterfaces = new List<Type>();
                if (typeInfo.BaseType != null) baseTypesAndInterfaces.Add(typeInfo.BaseType);
                baseTypesAndInterfaces.AddRange(type.GetInterfaces());
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

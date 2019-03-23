using System;
using System.Collections.Generic;
using System.Reflection;

namespace Cqrs.EntityFrameworkCore
{
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
    }
}

using Cqrs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Cqrs.JsonApi
{
    public static class TResourceExtensions
    {
        public static IEnumerable<PropertyInfo> GetRelationsProperties<TResource>()
            where TResource : IResource
        {
            var resorceType = typeof(IResource);
            return typeof(TResource).GetTypeInfo()
                .DeclaredProperties
                .Where(p => resorceType.IsAssignableFrom(p.PropertyType) || p.PropertyType.IsEnumerableOf<IResource>(out Type relationType));
        }

        public static IEnumerable<PropertyInfo> FindRelationsProperties<TResource>(string jsonApiResourceType)
            where TResource : IResource
        {
            return GetRelationsProperties<TResource>()
                       .Where(p =>
                       {
                           try
                           {
                               var relationType = p.PropertyType.IsEnumerableOf<IResource>(out Type relType) ? relType : p.PropertyType;
                               var relation = (IResource)Activator.CreateInstance(relationType);
                               return relation.Type == jsonApiResourceType;
                           }
                           catch
                           {
                               return false;
                           }
                       });
        }
    }
}

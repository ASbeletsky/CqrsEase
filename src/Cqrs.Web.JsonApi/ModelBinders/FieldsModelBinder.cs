using Cqrs.Common;
using Cqrs.Common.Queries;
using Cqrs.Common.Queries.FetchStrategies;
using Cqrs.JsonApi;
using Cqrs.Web.JsonApi.FetchStrategies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cqrs.Web.JsonApi.ModelBinders
{
    public class FieldsModelBinder<TResource> : IModelBinder
        where TResource : IResource, new()
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType == typeof(IFetchStrategy<TResource>))
            {
                var parameters = bindingContext.ActionContext.HttpContext.Request.QueryString;
                var fetchStrategy = default(IFetchStrategy<TResource>);
                if (parameters.HasValue)
                {
                    if(parameters.Value.Contains("fields"))
                    {
                        fetchStrategy = new FetchOnlyStrategy<TResource>(nameof(IResource.Id), nameof(IResource.Type));
                        var resourcesFields = ParseSparceFieldsets(parameters);
                        IncludeSparceFieldsets(fetchStrategy, resourcesFields);
                    }                    
                }
                else
                {
                    fetchStrategy = new FetchAllExceptRelationsStrategy<TResource>();
                }

                bindingContext.Model = fetchStrategy;
                bindingContext.Result = ModelBindingResult.Success(fetchStrategy);
            }

            return Task.CompletedTask;
        }

        private Dictionary<string, IEnumerable<string>> ParseSparceFieldsets(QueryString queryString)
        {
            var regex = new Regex(@"fields\[(?<key>[^.]+)\]=(?<value>[^.]+)");
            var fieldsKeyValues = new Dictionary<string, IEnumerable<string>>();
            queryString
                .Value
                .Substring(1)
                .Split('&')
                .Where(_ => _.StartsWith("fields"))
                .ToList()
                .ForEach(_ =>
                {
                    var groups = regex.Match(_).Groups;
                    if (groups.Count == 0) throw new ArgumentOutOfRangeException();
                    fieldsKeyValues.Add(groups["key"].Value, groups["value"].Value.Split(','));
                });

            return fieldsKeyValues;
        }

        private void IncludeSparceFieldsets(IFetchStrategy<TResource> fetchStrategy, Dictionary<string, IEnumerable<string>> resourcesFields)
        {
            var defaultResorce = new TResource();
            foreach(var resourceFields in resourcesFields)
            {
                var resourceType = resourceFields.Key;
                var properties = resourceFields.Value.Select(field => field.ToPascalCase());
                if (resourceType == defaultResorce.Type)
                {
                    fetchStrategy.IncludeRange(properties);
                }
                else
                {
                    var relationsFields = TResourceExtensions.FindRelationsProperties<TResource>(resourceType);
                    foreach (var relationFields in relationsFields)
                    {
                        var prefix = $"{relationFields.Name.ToPascalCase()}.";
                        fetchStrategy.Include(prefix + nameof(IResource.Id));
                        fetchStrategy.Include(prefix + nameof(IResource.Type));
                        properties = properties.Select(prop => prefix + prop);
                        fetchStrategy.IncludeRange(properties);
                    }
                }
            }
        }


    }
}

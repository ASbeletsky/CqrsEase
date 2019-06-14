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
        private string _rootResourceType;
        internal string RootResourceType
        {
            get
            {
                if (_rootResourceType == null)
                {
                    var defaultResorce = new TResource();
                    _rootResourceType = defaultResorce.Type;
                }

                return _rootResourceType;
            }
        }


        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType == typeof(IFetchStrategy<TResource>))
            {
                var query = bindingContext.ActionContext.HttpContext.Request.Query;
                var parameters = bindingContext.ActionContext.HttpContext.Request.QueryString;
                var fetchStrategy = default(IFetchStrategy<TResource>);

                bool hasSparceFieldsets = parameters.HasValue && parameters.Value.Contains("fields");
                bool hasIncudedRelations = query.ContainsKey("include");

                if (!hasSparceFieldsets && !hasIncudedRelations)
                {
                    fetchStrategy = new FetchAllExceptRelationsStrategy<TResource>();
                }

                if(!hasSparceFieldsets && hasIncudedRelations)
                {
                    var inlcudesResources = query["include"].First();
                    fetchStrategy = new FetchAllIncludingRelationsStrategy<TResource>(inlcudesResources.Split(','));
                }

                if(hasSparceFieldsets && !hasIncudedRelations)
                {
                    var rootFieldsParameter = $"fields[{RootResourceType}]";
                    var fields = query[rootFieldsParameter].FirstOrDefault() ?? throw new ArgumentException($"cannot parse {rootFieldsParameter} parameter");
                    var properties = fields.Split(',').Select(field => field.ToPascalCase()).ToArray();
                    fetchStrategy = new FetchOnlyStrategy<TResource>(properties);
                }

                if(hasSparceFieldsets && hasIncudedRelations)
                {
                    fetchStrategy = new FetchOnlyStrategy<TResource>(nameof(IResource.Id), nameof(IResource.Type));
                    var resourcesSparceFieldsets = ParseSparceFieldsets(parameters);
                    IncludeSparceFieldsets(fetchStrategy, resourcesSparceFieldsets);
                }

                bindingContext.Model = fetchStrategy;
                bindingContext.Result = ModelBindingResult.Success(fetchStrategy);
            }

            return Task.CompletedTask;
        }

        private Dictionary<string, IEnumerable<string>> ParseSparceFieldsets(QueryString queryString)
        {
            var regex = new Regex(@"fields\[(?<key>[^.]+)\]=(?<value>[^.]+)");
            var resourcesSparceFieldsets = new Dictionary<string, IEnumerable<string>>();
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
                    var resourceType = groups["key"].Value;
                    var resourceFields = groups["value"].Value.Split(',');
                    resourcesSparceFieldsets.Add(resourceType, resourceFields);
                });

            return resourcesSparceFieldsets;
        }

        private void IncludeSparceFieldsets(IFetchStrategy<TResource> fetchStrategy, Dictionary<string, IEnumerable<string>> resourcesFields)
        {
            
            foreach(var resourceFields in resourcesFields)
            {
                var resourceType = resourceFields.Key;
                var properties = resourceFields.Value.Select(field => field.ToPascalCase());
                if (resourceType == RootResourceType)
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

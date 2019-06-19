namespace CqrsEase.Web.JsonApi.ModelBinders
{
    #region Using
    using CqrsEase.Common;
    using CqrsEase.Common.Queries;
    using CqrsEase.Common.Queries.FetchStrategies;
    using CqrsEase.JsonApi;
    using CqrsEase.JsonApi.Web.Request;
    using CqrsEase.Web.JsonApi.FetchStrategies;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    #endregion

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

        internal string[] RequiredResourceFields
        {
            get
            {
                return new string[] { nameof(IResource.Id), nameof(IResource.Type) };
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
                    var inlcudedResourcesValue = query[JsonApiQueryParams.Include].FirstOrDefault();
                    if (string.IsNullOrWhiteSpace(inlcudedResourcesValue))
                        throw new ArgumentException($"cannot parse {JsonApiQueryParams.Include} parameter", JsonApiQueryParams.Include);

                    fetchStrategy = new FetchAllIncludingRelationsStrategy<TResource>(inlcudedResourcesValue.Split(','));
                }

                if(hasSparceFieldsets && !hasIncudedRelations)
                {
                    var resourcesSparceFieldsets = ParseSparceFieldsets(parameters);
                    var notIncludedRelation = resourcesSparceFieldsets.Keys.FirstOrDefault(resourceType => resourceType != RootResourceType);
                    if (notIncludedRelation != null)
                    {
                        var relationFieldsParameter = string.Format(JsonApiQueryParams.SparseFieldsetsTemplate, notIncludedRelation);
                        throw new ArgumentException($"SparceFieldsets for type {notIncludedRelation} was provided, but this type is not mentioned at {JsonApiQueryParams.Include} parameter", relationFieldsParameter);
                    }

                    if ( !resourcesSparceFieldsets.Any()
                        || !resourcesSparceFieldsets.ContainsKey(RootResourceType)
                        || !resourcesSparceFieldsets[RootResourceType].Any()
                        || resourcesSparceFieldsets[RootResourceType].Any(prop => string.IsNullOrWhiteSpace(prop)))
                    {
                        var rootFieldsParameter = string.Format(JsonApiQueryParams.SparseFieldsetsTemplate, RootResourceType);
                        throw new ArgumentException($"cannot parse {rootFieldsParameter} parameter", rootFieldsParameter);
                    }
                    
                    fetchStrategy = new FetchOnlyStrategy<TResource>(RequiredResourceFields);
                    IncludeSparceFieldsets(fetchStrategy, resourcesSparceFieldsets);
                }

                if(hasSparceFieldsets && hasIncudedRelations)
                {
                    fetchStrategy = new FetchOnlyStrategy<TResource>(RequiredResourceFields);
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
            var parseException = new ArgumentException("cannot parse fields parameter", "fields");
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
                    if (groups.Count == 0) throw parseException;
                    var resourceType = groups["key"].Value;
                    if (string.IsNullOrWhiteSpace(resourceType)) throw parseException;
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

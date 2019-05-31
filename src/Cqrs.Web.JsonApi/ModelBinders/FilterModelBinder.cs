namespace Cqrs.Web.JsonApi.ModelBinders
{
    using Cqrs.Common;
    #region Using
    using Cqrs.JsonApi;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using NSpecifications;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Net;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    #endregion

    public class FilterModelBinder<TResource> : IModelBinder
        where TResource : IResource
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType == typeof(ISpecification<TResource>))
            {
                var queryParameters = bindingContext.ActionContext.HttpContext.Request.QueryString;
                if (queryParameters.HasValue)
                {
                    try
                    {
                        var filterKeyValues = ParseQueryFilter(queryParameters);
                        var specifications = filterKeyValues.Select(kv =>
                        {
                            var memberName = kv.Key.ToPascalCase();
                            var memberType = typeof(TResource).GetTypeInfo().GetDeclaredProperty(memberName)?.PropertyType;
                            if (memberType == null)
                                throw new InvalidOperationException($"Resource {typeof(TResource).Name} doesn't contain field {kv.Key}");

                            var value = TypeDescriptor.GetConverter(memberType).ConvertFrom(kv.Value);
                            return SpecificationFactory<TResource>.CreateEqualitySpecification(memberName, value);
                        }).ToList();

                        var model = SpecificationFactory<TResource>.Сonjunction(specifications);
                        bindingContext.Model = model;
                        bindingContext.Result = ModelBindingResult.Success(model);
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Cannot parse filter parameter.");
                    }
                }
            }

            return Task.CompletedTask;
        }

        private Dictionary<string, string> ParseQueryFilter(QueryString queryString)
        {
            var regex = new Regex(@"filter\[(?<key>[^.]+)\]=(?<value>[^.]+)");
            var filterKeyValues = new Dictionary<string, string>();
            queryString
                .Value
                .Substring(1)
                .Split('&')
                .Where(_ => _.StartsWith("filter"))
                .ToList()
                .ForEach(_ =>
                {
                    var groups = regex.Match(_).Groups;
                    if (groups.Count == 0) throw new ArgumentOutOfRangeException();
                    filterKeyValues.Add(groups["key"].Value, WebUtility.UrlDecode(groups["value"].Value));
                });

            return filterKeyValues;
        }
    }
}

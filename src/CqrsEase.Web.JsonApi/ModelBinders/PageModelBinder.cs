namespace CqrsEase.Web.JsonApi.ModelBinders
{
    #region Using
    using CqrsEase.Common.Queries.Pagination;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    #endregion

    public class PageModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType == typeof(IPage))
            {
                var parameters = bindingContext.ActionContext.HttpContext.Request.QueryString;
                if (parameters.HasValue && parameters.Value.Contains("page"))
                {
                    bindingContext.Model = ParseQueryPage(parameters);
                }
            }

            return Task.CompletedTask;
        }

        private IPage ParseQueryPage(QueryString queryString)
        {
            var partialPager = new Dictionary<string, string>();
            var regex = new Regex(@"page\[(?<key>[\w]+)\]=(?<value>[\w^,]+)");
            queryString
                .Value
                .Substring(1)
                .Split('&')
                .Where(_ => _.StartsWith("page"))
                .ToList()
                .ForEach(_ =>
                {
                    var groups = regex.Match(_).Groups;
                    if (groups.Count == 0) throw new ArgumentOutOfRangeException();
                    partialPager.Add(groups["key"].Value, groups["value"].Value);
                });

            int pageNumber = Page.DefaultPageNumber;
            int pageSize = Page.DefaultPageSize;
            var hasNumber = partialPager.TryGetValue("number", out string pageNumberString) && int.TryParse(pageNumberString, out pageNumber);
            var hasSize = partialPager.TryGetValue("size", out string pageSizeString) && int.TryParse(pageSizeString, out pageSize);
            return new Page(pageNumber, pageSize);
        }

    }
}

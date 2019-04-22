using Cqrs.Common.Queries.Sorting;
using Cqrs.JsonApi;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cqrs.Web.JsonApi.ModelBinders
{
    public class SortModelBinder<TResource> : IModelBinder
        where TResource : IResource
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType == typeof(IEnumerable<OrderCreteria<TResource>>))
            {
                var sortParameters = bindingContext.ActionContext.HttpContext.Request.Query["sort"];
                if (sortParameters.Any())
                {
                    var sortParameter = sortParameters.First();
                    bindingContext.Model = ParseQuerySorting(sortParameter);
                    bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);
                } 
            }

            return Task.CompletedTask;
        }

        private IEnumerable<OrderCreteria<TResource>> ParseQuerySorting(string sortString)
        {
            var paths = sortString.Split(',').ToList();
            return paths.Select(path =>
            {
                char orderDirectionSymbol = path[0];
                bool hasImplictOrderDirection = orderDirectionSymbol == '-' || orderDirectionSymbol == '+';
                return hasImplictOrderDirection
                    ? new OrderCreteria<TResource>(FormatAsSelector(path.Substring(1)), (OrderDirection)orderDirectionSymbol)
                    : new OrderCreteria<TResource>(FormatAsSelector(path));
            });
        }

        private string FormatAsSelector(string path)
        {
            return path.Contains('.')
                    ? string.Join(".", path.Split('.').Select(p => p.ToPascalCase()))
                    : path.ToPascalCase();
        }
    }
}

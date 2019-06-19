using CqrsEase.Common.Queries;
using CqrsEase.Common.Queries.FetchStrategies;
using CqrsEase.Tests.Model;
using CqrsEase.Web.JsonApi.FetchStrategies;
using CqrsEase.Web.JsonApi.ModelBinders;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CqrsEase.Tests.Web
{
    public class FieldsModelBinderTests
    {
        [Fact]
        public async Task DoesNotParse_WhenTypeIsNotResource()
        {
            var binder = new FieldsModelBinder<BlogDto>();
            var contextMock = new Mock<ModelBindingContext>();
            contextMock.Setup(x => x.ModelType).Returns(typeof(int));

            await binder.BindModelAsync(contextMock.Object);

            Assert.Null(contextMock.Object.Model);
            Assert.Equal(ModelBindingResult.Failed(), contextMock.Object.Result);
        }

        [Fact]
        public async Task ReturnsFetchAllExceptRelationsStrategy_WhenQueryStringIsEmpty()
        {
            var httpRequestMock = new Mock<HttpRequest>();
            httpRequestMock.Setup(x => x.QueryString).Returns(QueryString.Empty);
            httpRequestMock.Setup(x => x.Query).Returns(new QueryCollection());

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(x => x.Request).Returns(httpRequestMock.Object);

            var actionContext = new ActionContext();
            actionContext.HttpContext = httpContextMock.Object;

            var bindingContextMock = new Mock<ModelBindingContext>();
            bindingContextMock.Setup(x => x.ModelType).Returns(typeof(IFetchStrategy<BlogDto>));
            bindingContextMock.Setup(x => x.HttpContext).Returns(httpContextMock.Object);
            bindingContextMock.Setup(x => x.ActionContext).Returns(actionContext);
            bindingContextMock.SetupProperty(x => x.Model);

            var binder = new FieldsModelBinder<BlogDto>();
            await binder.BindModelAsync(bindingContextMock.Object);

            Assert.IsType<FetchAllExceptRelationsStrategy<BlogDto>>(bindingContextMock.Object.Model);
        }

        [Theory]
        [InlineData("?fields[blog]=title", "Id", "Type", "Title")]
        [InlineData("?fields[blog]=id,type,title", "Id", "Type", "Title")]
        [InlineData("?include=author&fields[author]=name", "Id", "Type", "Author.Id", "Author.Type", "Author.Name")]
        [InlineData("?include=comment&fields[comment]=content", "Id", "Type", "Comments.Id", "Comments.Type", "Comments.Content")]
        [InlineData("?include=author,comment&fields[blog]=title&fields[author]=name&fields[comment]=content", "Id", "Type", "Title", "Author.Id", "Author.Type", "Author.Name", "Comments.Id", "Comments.Type", "Comments.Content")]
        public async Task IncludesRootResourceFields_WhenProvided(string query, params string[] expectedPaths)
        {
            var fieldsQueryString = new QueryString(query);
            var httpRequestMock = new Mock<HttpRequest>();
            httpRequestMock.Setup(x => x.QueryString).Returns(fieldsQueryString);
            httpRequestMock.Setup(x => x.Query).Returns(new QueryCollection(QueryHelpers.ParseQuery(query)));

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(x => x.Request).Returns(httpRequestMock.Object);

            var actionContext = new ActionContext();
            actionContext.HttpContext = httpContextMock.Object;

            var bindingContextMock = new Mock<ModelBindingContext>();
            bindingContextMock.Setup(x => x.ModelType).Returns(typeof(IFetchStrategy<BlogDto>));
            bindingContextMock.Setup(x => x.HttpContext).Returns(httpContextMock.Object);
            bindingContextMock.Setup(x => x.ActionContext).Returns(actionContext);
            bindingContextMock.SetupProperty(x => x.Model);

            var binder = new FieldsModelBinder<BlogDto>();
            await binder.BindModelAsync(bindingContextMock.Object);

            Assert.NotNull(bindingContextMock.Object.Model);
            Assert.IsType<FetchOnlyStrategy<BlogDto>>(bindingContextMock.Object.Model);

            var fetchStrategy = (IFetchStrategy<BlogDto>)bindingContextMock.Object.Model;
            Assert.NotEmpty(fetchStrategy.FetchedPaths);
            Assert.Collection(fetchStrategy.FetchedPaths, expectedPaths.Select(expectedPath => new Action<string>((actualPath) =>
            {
                Assert.Equal(expectedPath, actualPath);
            })).ToArray());
        }

        [Theory]
        [InlineData("?include=", "include")]
        [InlineData("?fields[blog]=", "fields")]
        [InlineData("?fields[blog]=id,", "fields[blog]")]
        [InlineData("?fields[author]=id,name", "fields[author]")]
        public async Task ThrowsException_WhenInvalidQueryStringProvided(string query, string parameterName)
        {
            var fieldsQueryString = new QueryString(query);
            var httpRequestMock = new Mock<HttpRequest>();
            httpRequestMock.Setup(x => x.QueryString).Returns(fieldsQueryString);
            httpRequestMock.Setup(x => x.Query).Returns(new QueryCollection(QueryHelpers.ParseQuery(query)));

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(x => x.Request).Returns(httpRequestMock.Object);

            var actionContext = new ActionContext();
            actionContext.HttpContext = httpContextMock.Object;

            var bindingContextMock = new Mock<ModelBindingContext>();
            bindingContextMock.Setup(x => x.ModelType).Returns(typeof(IFetchStrategy<BlogDto>));
            bindingContextMock.Setup(x => x.HttpContext).Returns(httpContextMock.Object);
            bindingContextMock.Setup(x => x.ActionContext).Returns(actionContext);
            bindingContextMock.SetupProperty(x => x.Model);

            var binder = new FieldsModelBinder<BlogDto>();
            await Assert.ThrowsAsync<ArgumentException>(parameterName, () =>  binder.BindModelAsync(bindingContextMock.Object));
        }

    }
}

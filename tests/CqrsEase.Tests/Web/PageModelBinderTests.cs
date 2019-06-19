namespace CqrsEase.Tests.Web
{
    #region Using
    using CqrsEase.Common.Queries.Pagination;
    using CqrsEase.Web.JsonApi.ModelBinders;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Moq;
    using System.Threading.Tasks;
    using Xunit;
    #endregion

    public class PageModelBinderTests
    {
        [Fact]
        public async Task DoesNotParse_WhenTypeIsNotResource()
        {
            var binder = new PageModelBinder();
            var contextMock = new Mock<ModelBindingContext>();
            contextMock.Setup(x => x.ModelType).Returns(typeof(int));

            await binder.BindModelAsync(contextMock.Object);

            Assert.Null(contextMock.Object.Model);
            Assert.Equal(ModelBindingResult.Failed(), contextMock.Object.Result);
        }

        [Fact]
        public async Task DoesNotParse_WhenQueryStringIsEmpty()
        {
            var httpRequestMock = new Mock<HttpRequest>();
            httpRequestMock.Setup(x => x.QueryString).Returns(QueryString.Empty);

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(x => x.Request).Returns(httpRequestMock.Object);

            var actionContext = new ActionContext();
            actionContext.HttpContext = httpContextMock.Object;

            var bindingContextMock = new Mock<ModelBindingContext>();
            bindingContextMock.Setup(x => x.ModelType).Returns(typeof(IPage));
            bindingContextMock.Setup(x => x.HttpContext).Returns(httpContextMock.Object);
            bindingContextMock.Setup(x => x.ActionContext).Returns(actionContext);

            var binder = new PageModelBinder();
            await binder.BindModelAsync(bindingContextMock.Object);

            Assert.Null(bindingContextMock.Object.Model);
            Assert.Equal(ModelBindingResult.Failed(), bindingContextMock.Object.Result);
        }

        [Theory]
        [InlineData("?page[number]=2&page[size]=5", 2, 5)]
        [InlineData("?page[number]=2", 2, Page.DefaultPageSize)]
        [InlineData("?page[size]=5", Page.DefaultPageNumber, 5)]
        public async Task CanParseQueryFilter_WithStringValue(string query, int pageNumber, int pageSize)
        {
            var filterQueryString = new QueryString(query);
            var httpRequestMock = new Mock<HttpRequest>();
            httpRequestMock.Setup(x => x.QueryString).Returns(filterQueryString);

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(x => x.Request).Returns(httpRequestMock.Object);

            var actionContext = new ActionContext();
            actionContext.HttpContext = httpContextMock.Object;

            var bindingContextMock = new Mock<ModelBindingContext>();
            bindingContextMock.Setup(x => x.ModelType).Returns(typeof(IPage));
            bindingContextMock.Setup(x => x.HttpContext).Returns(httpContextMock.Object);
            bindingContextMock.Setup(x => x.ActionContext).Returns(actionContext);
            bindingContextMock.SetupProperty(x => x.Model);

            var binder = new PageModelBinder();
            await binder.BindModelAsync(bindingContextMock.Object);

            Assert.NotNull(bindingContextMock.Object.Model);
            Assert.True(bindingContextMock.Object.Model is IPage);

            var page = bindingContextMock.Object.Model as IPage;
            Assert.Equal(pageNumber, page.Number);
            Assert.Equal(pageSize, page.Size);
        }
    }
}

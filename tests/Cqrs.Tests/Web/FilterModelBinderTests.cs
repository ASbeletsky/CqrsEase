namespace Cqrs.Tests.Web
{
    using Cqrs.Common.Queries;
    #region Using
    using Cqrs.Tests.Model;
    using Cqrs.Web.JsonApi.ModelBinders;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Moq;
    using NSpecifications;
    using System.Threading.Tasks;
    using Xunit;
    #endregion

    public class FilterModelBinderTests
    {
        [Fact]
        public async Task DoesNotParse_WhenTypeIsNotResource()
        {
            var binder = new FilterModelBinder<BlogDto>();
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
            bindingContextMock.Setup(x => x.ModelType).Returns(typeof(IFetchStrategy<BlogDto>));
            bindingContextMock.Setup(x => x.HttpContext).Returns(httpContextMock.Object);
            bindingContextMock.Setup(x => x.ActionContext).Returns(actionContext);

            var binder = new FieldsModelBinder<BlogDto>();
            await binder.BindModelAsync(bindingContextMock.Object);

            Assert.Null(bindingContextMock.Object.Model);
            Assert.Equal(ModelBindingResult.Failed(), bindingContextMock.Object.Result);
        }

        [Theory()]
        [InlineData("?filter[title]=test", "x => (x.Title == \"test\")")]
        [InlineData("?filter[title]=two+words", "x => (x.Title == \"two words\")")]
        [InlineData("?filter[title]=test&filter[id]=1", "x => ((x.Title == \"test\") AndAlso (x.Id == \"1\"))")]
        public async Task CanParseQueryFilter_WithStringValue(string query, string expectedSpecExpression)
        {
            var filterQueryString = new QueryString(query);
            var httpRequestMock = new Mock<HttpRequest>();
            httpRequestMock.Setup(x => x.QueryString).Returns(filterQueryString);

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(x => x.Request).Returns(httpRequestMock.Object);

            var actionContext = new ActionContext();
            actionContext.HttpContext = httpContextMock.Object;

            var bindingContextMock = new Mock<ModelBindingContext>();
            bindingContextMock.Setup(x => x.ModelType).Returns(typeof(ISpecification<BlogDto>));
            bindingContextMock.Setup(x => x.HttpContext).Returns(httpContextMock.Object);
            bindingContextMock.Setup(x => x.ActionContext).Returns(actionContext);
            bindingContextMock.SetupProperty(x => x.Model);

            var binder = new FilterModelBinder<BlogDto>();
            await binder.BindModelAsync(bindingContextMock.Object);

            Assert.NotNull(bindingContextMock.Object.Model);
            Assert.True(bindingContextMock.Object.Model is ASpec<BlogDto>);

            var spec = bindingContextMock.Object.Model as ASpec<BlogDto>;
            Assert.Equal(expectedSpecExpression, spec.Expression.ToString());
        }
    }
}

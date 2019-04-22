namespace Cqrs.Tests.Web
{
    #region Using
    using Cqrs.Common.Queries.Sorting;
    using Cqrs.Tests.Model;
    using Cqrs.Web.JsonApi.ModelBinders;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Internal;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.Extensions.Primitives;
    using Moq;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    #endregion

    public class SortModelBinderTests
    {
        [Fact]
        public async Task DoesNotParse_WhenTypeIsNotResource()
        {
            var binder = new SortModelBinder<BlogDto>();
            var contextMock = new Mock<ModelBindingContext>();
            contextMock.Setup(x => x.ModelType).Returns(typeof(int));

            await binder.BindModelAsync(contextMock.Object);

            Assert.Null(contextMock.Object.Model);
            Assert.Equal(ModelBindingResult.Failed(), contextMock.Object.Result);
        }

        [Fact]
        public async Task DoesNotParse_WhenQueryStringIsEmpty()
        {
            var query = new Dictionary<string, StringValues>();

            var httpRequestMock = new Mock<HttpRequest>();
            httpRequestMock.Setup(x => x.Query).Returns(new QueryCollection(query));

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(x => x.Request).Returns(httpRequestMock.Object);

            var actionContext = new ActionContext();
            actionContext.HttpContext = httpContextMock.Object;

            var bindingContextMock = new Mock<ModelBindingContext>();
            bindingContextMock.Setup(x => x.ModelType).Returns(typeof(IEnumerable<OrderCreteria<BlogDto>>));
            bindingContextMock.Setup(x => x.HttpContext).Returns(httpContextMock.Object);
            bindingContextMock.Setup(x => x.ActionContext).Returns(actionContext);

            var binder = new SortModelBinder<BlogDto>();
            await binder.BindModelAsync(bindingContextMock.Object);

            Assert.Null(bindingContextMock.Object.Model);
            Assert.Equal(ModelBindingResult.Failed(), bindingContextMock.Object.Result);
        }

        [Fact]     
        public async Task CanParse_WithImplictDirection()
        {
            var query = new Dictionary<string, StringValues>
            {
                { "sort", "title" }
            };

            var httpRequestMock = new Mock<HttpRequest>();
            httpRequestMock.Setup(x => x.Query).Returns(new QueryCollection(query));

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(x => x.Request).Returns(httpRequestMock.Object);

            var actionContext = new ActionContext();
            actionContext.HttpContext = httpContextMock.Object;

            var bindingContextMock = new Mock<ModelBindingContext>();
            bindingContextMock.Setup(x => x.ModelType).Returns(typeof(IEnumerable<OrderCreteria<BlogDto>>));
            bindingContextMock.Setup(x => x.HttpContext).Returns(httpContextMock.Object);
            bindingContextMock.Setup(x => x.ActionContext).Returns(actionContext);
            bindingContextMock.SetupProperty(x => x.Model);

            var binder = new SortModelBinder<BlogDto>();
            await binder.BindModelAsync(bindingContextMock.Object);

            Assert.NotNull(bindingContextMock.Object.Model);
            Assert.True(bindingContextMock.Object.Model is IEnumerable<OrderCreteria<BlogDto>>);

            var ordering = bindingContextMock.Object.Model as IEnumerable<OrderCreteria<BlogDto>>;
            Assert.NotEmpty(ordering);
            Assert.Single(ordering);

            var orderCreteria = ordering.Single();
            Assert.Equal("Title", orderCreteria.SortKey);
            Assert.Equal(OrderDirection.ASC, orderCreteria.Direction);
        }

        [Fact]
        public async Task CanParse_WithMultipleFields()
        {
            var query = new Dictionary<string, StringValues>
            {
                { "sort", "-title,+id" }
            };

            var httpRequestMock = new Mock<HttpRequest>();
            httpRequestMock.Setup(x => x.Query).Returns(new QueryCollection(query));

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(x => x.Request).Returns(httpRequestMock.Object);

            var actionContext = new ActionContext();
            actionContext.HttpContext = httpContextMock.Object;

            var bindingContextMock = new Mock<ModelBindingContext>();
            bindingContextMock.Setup(x => x.ModelType).Returns(typeof(IEnumerable<OrderCreteria<BlogDto>>));
            bindingContextMock.Setup(x => x.HttpContext).Returns(httpContextMock.Object);
            bindingContextMock.Setup(x => x.ActionContext).Returns(actionContext);
            bindingContextMock.SetupProperty(x => x.Model);

            var binder = new SortModelBinder<BlogDto>();
            await binder.BindModelAsync(bindingContextMock.Object);

            Assert.NotNull(bindingContextMock.Object.Model);
            Assert.True(bindingContextMock.Object.Model is IEnumerable<OrderCreteria<BlogDto>>);

            var ordering = bindingContextMock.Object.Model as IEnumerable<OrderCreteria<BlogDto>>;
            Assert.NotEmpty(ordering);
            Assert.Equal(2, ordering.Count());

            Assert.Collection(ordering,
                (creteria1 => { Assert.Equal("Title", creteria1.SortKey); Assert.Equal(OrderDirection.DESC, creteria1.Direction); }),
                (creteria2 => { Assert.Equal("Id", creteria2.SortKey); Assert.Equal(OrderDirection.ASC, creteria2.Direction); })
            );
        }

        [Fact]
        public async Task CanParse_WithComplexPath()
        {
            var query = new Dictionary<string, StringValues>
            {
                { "sort", "-author.name,title,+author.Id" }
            };

            var httpRequestMock = new Mock<HttpRequest>();
            httpRequestMock.Setup(x => x.Query).Returns(new QueryCollection(query));

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(x => x.Request).Returns(httpRequestMock.Object);

            var actionContext = new ActionContext();
            actionContext.HttpContext = httpContextMock.Object;

            var bindingContextMock = new Mock<ModelBindingContext>();
            bindingContextMock.Setup(x => x.ModelType).Returns(typeof(IEnumerable<OrderCreteria<BlogDto>>));
            bindingContextMock.Setup(x => x.HttpContext).Returns(httpContextMock.Object);
            bindingContextMock.Setup(x => x.ActionContext).Returns(actionContext);
            bindingContextMock.SetupProperty(x => x.Model);

            var binder = new SortModelBinder<BlogDto>();
            await binder.BindModelAsync(bindingContextMock.Object);

            Assert.NotNull(bindingContextMock.Object.Model);
            Assert.True(bindingContextMock.Object.Model is IEnumerable<OrderCreteria<BlogDto>>);

            var ordering = bindingContextMock.Object.Model as IEnumerable<OrderCreteria<BlogDto>>;
            Assert.NotEmpty(ordering);
            Assert.Equal(3, ordering.Count());

            Assert.Collection(ordering,
                (creteria1 => { Assert.Equal("Author.Name", creteria1.SortKey); Assert.Equal(OrderDirection.DESC, creteria1.Direction); }),
                (creteria2 => { Assert.Equal("Title", creteria2.SortKey); Assert.Equal(OrderDirection.ASC, creteria2.Direction); }),
                (creteria3 => { Assert.Equal("Author.Id", creteria3.SortKey); Assert.Equal(OrderDirection.ASC, creteria3.Direction); })
             );
        }
    }
}

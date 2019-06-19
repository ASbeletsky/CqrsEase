namespace CqrsEase.Tests.JsonApi
{
    #region Using
    using CqrsEase.Common.Queries;
    using CqrsEase.Common.Queries.FetchStrategies;
    using CqrsEase.Common.Queries.Pagination;
    using CqrsEase.Common.Queries.Sorting;
    using CqrsEase.JsonApi.QueryHandlers;
    using CqrsEase.Tests.Model;
    using Microsoft.AspNetCore.WebUtilities;
    using NSpecifications;
    using System.Net;
    using System.Reflection;
    using Xunit;
    #endregion

    public class GetManyQueryHandlerTests
    {
        [Fact]
        public void AppliesSpecification_WhenProvided()
        {
            var expectedBlogTitle = "second blog";
            var spec = new Spec<BlogDto>(b => b.Title == expectedBlogTitle);
            var query = new GetManyQuery<BlogDto>(spec);
            var handler = new GetManyQueryHandler<BlogDto>("http://test.com/blogs");
            var buildQueryStringMethod = handler.GetType().GetTypeInfo().GetMethod("BuildQueryString", BindingFlags.Instance | BindingFlags.NonPublic);

            var encodedQueryString = (string)buildQueryStringMethod.Invoke(handler, new object[] { query });
            var actualQueryString = WebUtility.UrlDecode(encodedQueryString);
            var queryParameters = QueryHelpers.ParseQuery(actualQueryString);

            Assert.NotNull(actualQueryString);
            Assert.Contains(queryParameters, p => p.Key == "filter[title]");
            Assert.Equal("second blog", queryParameters["filter[title]"]);
        }

        [Fact]
        public void AppliesSorting_WhenProvided()
        {
            var query = new GetManyQuery<BlogDto>(
            new OrderCreteria<BlogDto>(b => b.Title, OrderDirection.ASC),
            new OrderCreteria<BlogDto>(b => b.Comments, OrderDirection.DESC));

            var handler = new GetManyQueryHandler<BlogDto>("http://test.com/blogs");
            var buildQueryStringMethod = handler.GetType().GetTypeInfo().GetMethod("BuildQueryString", BindingFlags.Instance | BindingFlags.NonPublic);

            var encodedQueryString = (string)buildQueryStringMethod.Invoke(handler, new object[] { query });
            var actualQueryString = WebUtility.UrlDecode(encodedQueryString);
            var queryParameters = QueryHelpers.ParseQuery(actualQueryString);

            Assert.NotNull(actualQueryString);
            Assert.Contains(queryParameters, p => p.Key == "sort");
            Assert.Equal("title,-comments", queryParameters["sort"]);
        }

        [Fact]
        public void DoesNotIncludeSparseFieldsets_WhenFetchStrategyNotProvided()
        {
            var query = new GetManyQuery<BlogDto>();
            var handler = new GetManyQueryHandler<BlogDto>("http://test.com/blogs");
            var buildQueryStringMethod = handler.GetType().GetTypeInfo().GetMethod("BuildQueryString", BindingFlags.Instance | BindingFlags.NonPublic);

            var encodedQueryString = (string)buildQueryStringMethod.Invoke(handler, new object[] { query });
            var actualQueryString = WebUtility.UrlDecode(encodedQueryString);
            var queryParameters = QueryHelpers.ParseQuery(actualQueryString);

            Assert.DoesNotContain(queryParameters, p => p.Key == "fields[blog]");
            Assert.DoesNotContain(actualQueryString, "fields");
        }

        [Fact]
        public void AppliesSparseFieldsets_WhenFetchStrategyProvided()
        {
            var query = new GetManyQuery<BlogDto>(new FetchOnlyStrategy<BlogDto>(b => b.Id, b => b.Title));

            var handler = new GetManyQueryHandler<BlogDto>("http://test.com/blogs");
            var buildQueryStringMethod = handler.GetType().GetTypeInfo().GetMethod("BuildQueryString", BindingFlags.Instance | BindingFlags.NonPublic);

            var encodedQueryString = (string)buildQueryStringMethod.Invoke(handler, new object[] { query });
            var actualQueryString = WebUtility.UrlDecode(encodedQueryString);
            var queryParameters = QueryHelpers.ParseQuery(actualQueryString);

            Assert.NotNull(actualQueryString);
            Assert.Contains(queryParameters, p => p.Key == "fields[blog]");
            Assert.Equal("id,title", queryParameters["fields[blog]"]);
        }

        [Fact]
        public void IncludesRelatedResources_WhenProvided()
        {
            var query = new GetManyQuery<BlogDto>(new FetchOnlyStrategy<BlogDto>(b => b.Id, b => b.Author));

            var handler = new GetManyQueryHandler<BlogDto>("http://test.com/blogs");
            var buildQueryStringMethod = handler.GetType().GetTypeInfo().GetMethod("BuildQueryString", BindingFlags.Instance | BindingFlags.NonPublic);

            var encodedQueryString = (string)buildQueryStringMethod.Invoke(handler, new object[] { query });
            var actualQueryString = WebUtility.UrlDecode(encodedQueryString);
            var queryParameters = QueryHelpers.ParseQuery(actualQueryString);

            Assert.NotNull(actualQueryString);
            Assert.Contains(queryParameters, p => p.Key == "include");
            Assert.Equal("author", queryParameters["include"]);
            Assert.Contains(queryParameters, p => p.Key == "fields[blog]");
            Assert.Equal("id,author", queryParameters["fields[blog]"]);
        }

        [Fact]
        public void AppliesPageSize_WhenProvided()
        {
            var query = new GetManyQuery<BlogDto>(new Page(2, 30));

            var handler = new GetManyQueryHandler<BlogDto>("http://test.com/blogs");
            var buildQueryStringMethod = handler.GetType().GetTypeInfo().GetMethod("BuildQueryString", BindingFlags.Instance | BindingFlags.NonPublic);

            var encodedQueryString = (string)buildQueryStringMethod.Invoke(handler, new object[] { query });
            var actualQueryString = WebUtility.UrlDecode(encodedQueryString);
            var queryParameters = QueryHelpers.ParseQuery(actualQueryString);

            Assert.NotNull(actualQueryString);
            Assert.Contains(queryParameters, p => p.Key == "page[size]");
            Assert.Equal("30", queryParameters["page[size]"]);
        }

        [Fact]
        public void AppliesPageNumber_WhenProvided()
        {
            var query = new GetManyQuery<BlogDto>(new Page(2, 30));

            var handler = new GetManyQueryHandler<BlogDto>("http://test.com/blogs");
            var buildQueryStringMethod = handler.GetType().GetTypeInfo().GetMethod("BuildQueryString", BindingFlags.Instance | BindingFlags.NonPublic);

            var encodedQueryString = (string)buildQueryStringMethod.Invoke(handler, new object[] { query });
            var actualQueryString = WebUtility.UrlDecode(encodedQueryString);
            var queryParameters = QueryHelpers.ParseQuery(actualQueryString);

            Assert.NotNull(actualQueryString);
            Assert.Contains(queryParameters, p => p.Key == "page[number]");
            Assert.Equal("2", queryParameters["page[number]"]);
        }
    }
}

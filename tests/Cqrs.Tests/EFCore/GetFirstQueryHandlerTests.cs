using Cqrs.Common.Queries;
using Cqrs.Common.Queries.FetchStateries;
using Cqrs.Core;
using Cqrs.EntityFrameworkCore;
using Cqrs.EntityFrameworkCore.QueryHandlers;
using Cqrs.Tests.Model;
using Microsoft.EntityFrameworkCore;
using NSpecifications;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Cqrs.Tests.EFCore
{
    public class GetFirstQueryHandlerTests
    {
        [Fact]
        public void AppliesSpecification_WhenProvided()
        {
            var options = new DbContextOptionsBuilder<BloggingContext>().UseInMemoryDatabase(databaseName: "AppliesSpecification_WhenProvided").Options;
            var context = new BloggingContext(options);
            using (context)
            {
                context.Blogs.AddRange(new Blog { Title = "first blog" }, new Blog { Title = "second blog" }, new Blog { Title = "third blog" });
                context.SaveChanges();

                var expectedBlogTitle = "second blog";
                var spec = new Spec<Blog>(b => b.Title == expectedBlogTitle);
                var query = new GetFirstQuery<Blog>(spec);
                var queryHandler = new GetFirstQueryHandler<Blog>(new EfDataSourceBased(context));
                var blog = queryHandler.Request(query);

                Assert.NotNull(blog);
                Assert.Equal(expectedBlogTitle, blog.Title);
            }
        }

        [Fact]
        public void AppliesSorting_WhenProvided_AsString()
        {
            var options = new DbContextOptionsBuilder<BloggingContext>().UseInMemoryDatabase(databaseName: "AppliesSorting_WhenProvided_AsString").Options;
            var context = new BloggingContext(options);
            using (context)
            {
                context.Blogs.AddRange(new Blog { Id = 3, Title = "same blog title" }, new Blog { Id = 2, Title = "same blog title" }, new Blog { Id = 1, Title = "same blog title" });
                context.SaveChanges();

                var expectedBlogId = 1;
                var spec = new Spec<Blog>(b => b.Title == "same blog title");
                var query = new GetFirstQuery<Blog>(spec, orderBy: nameof(Blog.Id));
                var queryHandler = new GetFirstQueryHandler<Blog>(new EfDataSourceBased(context));
                var blog = queryHandler.Request(query);

                Assert.NotNull(blog);
                Assert.Equal(expectedBlogId, blog.Id);
            }
        }

        [Fact]
        public void AppliesSorting_WhenProvided_AsExpression()
        {
            var options = new DbContextOptionsBuilder<BloggingContext>().UseInMemoryDatabase(databaseName: "AppliesSorting_WhenProvided_AsExpression").Options;
            var context = new BloggingContext(options);
            using (context)
            {
                context.Blogs.AddRange(new Blog { Id = 3, Title = "same blog title" }, new Blog { Id = 2, Title = "same blog title" }, new Blog { Id = 1, Title = "same blog title" });
                context.SaveChanges();

                var expectedBlogId = 1;
                var spec = new Spec<Blog>(b => b.Title == "same blog title");
                var query = new GetFirstQuery<Blog>(spec, orderBy: x => x.Id);
                var queryHandler = new GetFirstQueryHandler<Blog>(new EfDataSourceBased(context));
                var blog = queryHandler.Request(query);

                Assert.NotNull(blog);
                Assert.Equal(expectedBlogId, blog.Id);
            }
        }

        [Fact]
        public void SelectsAllFileds_WhenFetchStrategyNotProvided()
        {
            var options = new DbContextOptionsBuilder<BloggingContext>().UseInMemoryDatabase(databaseName: "SelectsAllFileds_WhenFetchStrategyNotProvided").Options;
            var context = new BloggingContext(options);
            using (context)
            {
                var expectedBlog = new Blog { Id = 3, Title = "some blog title" };
                context.Blogs.Add(expectedBlog);
                context.SaveChanges();

                var query = new GetFirstQuery<Blog>(new Spec<Blog>(b => b.Id > 0));
                var queryHandler = new GetFirstQueryHandler<Blog>(new EfDataSourceBased(context));
                var actualBlog = queryHandler.Request(query);

                Assert.NotNull(actualBlog);
                Assert.Equal(expectedBlog.Id, actualBlog.Id);
                Assert.Equal(expectedBlog.Title, actualBlog.Title);
            }
        }

        [Fact]
        public void SelectsOnlyNeededFileds_WhenFetchStrategyProvided()
        {
            var options = new DbContextOptionsBuilder<BloggingContext>().UseInMemoryDatabase(databaseName: "SelectsOnlyNeededFileds_WhenFetchStrategyProvided").Options;
            var context = new BloggingContext(options);
            using (context)
            {
                var expectedBlog = new Blog { Id = 3, Title = "some blog title" };
                context.Blogs.Add(expectedBlog);
                context.SaveChanges();

                var someSpec = new Spec<Blog>(b => b.Id > 0);
                var includeOnlyId = new FetchOnlyStatery<Blog>((b => b.Id));
                var query = new GetFirstQuery<Blog>(someSpec, includeOnlyId);
                var queryHandler = new GetFirstQueryHandler<Blog>(new EfDataSourceBased(context));
                var actualBlog = queryHandler.Request(query);

                Assert.NotNull(actualBlog);
                Assert.Equal(expectedBlog.Id, actualBlog.Id);
                Assert.Null(actualBlog.Title);
            }
        }
    }
}

namespace Cqrs.Tests.EFCore
{
    #region Using
    using Cqrs.Common.Queries;
    using Cqrs.Common.Queries.FetchStateries;
    using Cqrs.EntityFrameworkCore.DataSource;
    using Cqrs.EntityFrameworkCore.QueryHandlers;
    using Cqrs.Tests.Model;
    using Microsoft.EntityFrameworkCore;
    using NSpecifications;
    using System.Collections.Generic;
    using Xunit;
    #endregion

    public class GetFirstQueryHandlerTests
    {
        [Fact]
        public void AppliesSpecification_WhenProvided()
        {
            var options = new DbContextOptionsBuilder<BloggingContext>().UseInMemoryDatabase(databaseName: "GetFirstQueryHandler_AppliesSpecification_WhenProvided").Options;
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
            var options = new DbContextOptionsBuilder<BloggingContext>().UseInMemoryDatabase(databaseName: "GetFirstQueryHandler_AppliesSorting_WhenProvided_AsString").Options;
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
            var options = new DbContextOptionsBuilder<BloggingContext>().UseInMemoryDatabase(databaseName: "GetFirstQueryHandler_AppliesSorting_WhenProvided_AsExpression").Options;
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
            var options = new DbContextOptionsBuilder<BloggingContext>().UseInMemoryDatabase(databaseName: "GetFirstQueryHandler_SelectsAllFileds_WhenFetchStrategyNotProvided").Options;
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
            var options = new DbContextOptionsBuilder<BloggingContext>().UseInMemoryDatabase(databaseName: "GetFirstQueryHandler_SelectsOnlyNeededFileds_WhenFetchStrategyProvided").Options;
            var context = new BloggingContext(options);
            using (context)
            {
                var expectedBlog = new Blog { Id = 3, Title = "some blog title" };
                context.Blogs.Add(expectedBlog);
                context.SaveChanges();

                var includeOnlyId = new FetchOnlyStratery<Blog>((b => b.Id));
                var query = new GetFirstQuery<Blog>(includeOnlyId);
                var queryHandler = new GetFirstQueryHandler<Blog>(new EfDataSourceBased(context));
                var actualBlog = queryHandler.Request(query);

                Assert.NotNull(actualBlog);
                Assert.Equal(expectedBlog.Id, actualBlog.Id);
                Assert.Null(actualBlog.Title);
            }
        }

        [Fact]
        public void DoesNotIncludeRelatedEntities_WhenFetchStrategyNotProvided()
        {
            var options = new DbContextOptionsBuilder<BloggingContext>().UseInMemoryDatabase(databaseName: "GetFirstQueryHandler_DoesNotIncludeRelatedEntities_WhenFetchStrategyNotProvided").Options;
            var context = new BloggingContext(options);
            using (context)
            {
                var blog = new Blog { Title = "some title" };
                context.Blogs.Add(blog);
                context.SaveChanges();

                var blogComments = new List<Comment>()
                {
                    new Comment { BlogId = blog.Id, Content = "first comment"},
                    new Comment { BlogId = blog.Id, Content = "second comment"},
                };

                context.Comments.AddRange(blogComments);
                context.SaveChanges();

                var getFirstBlogQuery = new GetFirstQuery<Blog>(orderBy: b => b.Id);
                var queryHandler = new GetFirstQueryHandler<Blog>(new EfDataSourceBased(context));
                var loadedBlog = queryHandler.Request(getFirstBlogQuery);
                    
                Assert.NotNull(loadedBlog);
                Assert.True(loadedBlog.Comments == null || context.Entry(loadedBlog).Collection(b => b.Comments).IsLoaded == false);
            }
        }

        [Fact]
        public void IncludesRelatedEntities_WhenFetchProvided()
        {
            var options = new DbContextOptionsBuilder<BloggingContext>().UseInMemoryDatabase(databaseName: "GetFirstQueryHandler_IncludesRelatedEntities_WhenFetchProvided").Options;
            var context = new BloggingContext(options);
            using (context)
            {
                var blog = new Blog { Title = "some title" };
                context.Blogs.Add(blog);
                context.SaveChanges();

                var blogComments = new List<Comment>()
                {
                    new Comment { BlogId = blog.Id, Content = "first comment"},
                    new Comment { BlogId = blog.Id, Content = "second comment"},
                };

                context.Comments.AddRange(blogComments);
                context.SaveChanges();

                var includeIdAndComments = new FetchOnlyStratery<Blog>(b => b.Id, b => b.Comments);
                var getFirstBlogQuery = new GetFirstQuery<Blog>(includeIdAndComments, orderBy: b => b.Id);
                var queryHandler = new GetFirstQueryHandler<Blog>(new EfDataSourceBased(context));
                var loadedBlog = queryHandler.Request(getFirstBlogQuery);

                Assert.NotNull(loadedBlog);
                Assert.Equal(blog.Id, loadedBlog.Id);
                Assert.Null(loadedBlog.Title);
                Assert.Equal(blogComments, loadedBlog.Comments);
            }
        }
    }
}

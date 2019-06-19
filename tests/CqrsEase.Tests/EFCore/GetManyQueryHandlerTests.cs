namespace CqrsEase.Tests.EFCore
{
    #region Using
    using CqrsEase.Common.Queries;
    using CqrsEase.Common.Queries.FetchStrategies;
    using CqrsEase.Common.Queries.Pagination;
    using CqrsEase.Common.Queries.Sorting;
    using CqrsEase.Core.Abstractions;
    using CqrsEase.EntityFrameworkCore.DataSource;
    using CqrsEase.EntityFrameworkCore.FetchStrategies;
    using CqrsEase.EntityFrameworkCore.QueryHandlers;
    using CqrsEase.Tests.Model;
    using Microsoft.EntityFrameworkCore;
    using NSpecifications;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;
    #endregion

    public class GetManyQueryHandlerTests
    {
        [Fact]
        public void AppliesSpecification_WhenProvided()
        {
            var options = new DbContextOptionsBuilder<BloggingContext>().UseInMemoryDatabase(databaseName: "GetManyQueryHandler_AppliesSpecification_WhenProvided").Options;
            var context = new BloggingContext(options);
            using (context)
            {
                context.Blogs.AddRange(new Blog { Title = "first blog" }, new Blog { Title = "cool blog" }, new Blog { Title = "cool blog" });
                context.SaveChanges();
                var spec = new Spec<Blog>(b => b.Title == "cool blog");
                var query = new GetManyQuery<Blog>(spec);
                var queryHandler = new GetManyQueryHandler<Blog>(new EfDataSourceBased(context), new FetchAllStrategy<Blog>());
                var blogs = queryHandler.Request(query);

                Assert.NotNull(blogs);
                Assert.Equal(2, blogs.Count());
                Assert.All(blogs, b =>
                {
                    Assert.NotNull(b);
                    Assert.True(spec.IsSatisfiedBy(b));
                });
            }
        }

        [Fact]
        public void AppliesSorting_WhenProvided()
        {
            var options = new DbContextOptionsBuilder<BloggingContext>().UseInMemoryDatabase(databaseName: "GetManyQueryHandler_AppliesSorting_WhenProvided").Options;
            var context = new BloggingContext(options);
            using (context)
            {
                var firstBlog = new Blog { Id = 1, Title = "blog title" };
                var secondBlog = new Blog { Id = 2, Title = "blog title" };
                var thirdBlog = new Blog { Id = 3, Title = "blog title" };
                context.Blogs.AddRange(firstBlog, secondBlog, thirdBlog);
                context.SaveChanges();

                var orderByIdDesc = new OrderCreteria<Blog>(nameof(Blog.Id), OrderDirection.DESC);
                var query = new GetManyQuery<Blog>(orderByIdDesc);
                var queryHandler = new GetManyQueryHandler<Blog>(new EfDataSourceBased(context), new FetchAllStrategy<Blog>());
                var blogs = queryHandler.Request(query);

                Assert.NotNull(blogs);
                Assert.Equal(3, blogs.Count());
                Assert.Collection(blogs,
                    b1 => Assert.Equal(b1.Id, thirdBlog.Id),
                    b2 => Assert.Equal(b2.Id, secondBlog.Id),
                    b3 => Assert.Equal(b3.Id, firstBlog.Id)
                );
            }
        }

        [Fact]
        public void SelectsAllFileds_WhenFetchStrategyNotProvided()
        {
            var options = new DbContextOptionsBuilder<BloggingContext>().UseInMemoryDatabase(databaseName: "GetManyQueryHandler_SelectsAllFileds_WhenFetchStrategyNotProvided").Options;
            var context = new BloggingContext(options);
            using (context)
            {
                var firstBlog = new Blog { Id = 1, Title = "blog title" };
                var secondBlog = new Blog { Id = 2, Title = "blog title" };
                var thirdBlog = new Blog { Id = 3, Title = "blog title" };
                context.Blogs.AddRange(firstBlog, secondBlog, thirdBlog);
                context.SaveChanges();

                var query = new GetManyQuery<Blog>();
                var queryHandler = new GetManyQueryHandler<Blog>(new EfDataSourceBased(context), new FetchAllStrategy<Blog>());
                var blogs = queryHandler.Request(query);

                Assert.NotNull(blogs);
                Assert.Equal(3, blogs.Count());
                Assert.Collection(blogs,
                    b1 => { Assert.Equal(b1.Id, firstBlog.Id); Assert.Equal(b1.Title, firstBlog.Title); },
                    b2 => { Assert.Equal(b2.Id, secondBlog.Id); Assert.Equal(b2.Title, secondBlog.Title); },
                    b3 => { Assert.Equal(b3.Id, thirdBlog.Id); Assert.Equal(b3.Title, thirdBlog.Title); }
                );
            }
        }

        [Fact]
        public void SelectsOnlyNeededFileds_WhenFetchStrategyProvided()
        {
            var options = new DbContextOptionsBuilder<BloggingContext>().UseInMemoryDatabase(databaseName: "GetManyQueryHandler_SelectsOnlyNeededFileds_WhenFetchStrategyProvided").Options;
            var context = new BloggingContext(options);
            using (context)
            {
                var firstBlog = new Blog { Id = 1, Title = "blog title" };
                var secondBlog = new Blog { Id = 2, Title = "blog title" };
                var thirdBlog = new Blog { Id = 3, Title = "blog title" };
                context.Blogs.AddRange(firstBlog, secondBlog, thirdBlog);
                context.SaveChanges();

                var includeOnlyId = new FetchOnlyStrategy<Blog>((b => b.Id));
                var query = new GetManyQuery<Blog>(includeOnlyId);
                var queryHandler = new GetManyQueryHandler<Blog>(new EfDataSourceBased(context), new FetchAllStrategy<Blog>());
                var blogs = queryHandler.Request(query);

                Assert.NotNull(blogs);
                Assert.Equal(3, blogs.Count());
                Assert.Collection(blogs,
                    b1 => { Assert.Equal(b1.Id, firstBlog.Id); Assert.Null(b1.Title); Assert.Null(b1.Comments); },
                    b2 => { Assert.Equal(b2.Id, secondBlog.Id); Assert.Null(b2.Title); Assert.Null(b2.Comments); },
                    b3 => { Assert.Equal(b3.Id, thirdBlog.Id); Assert.Null(b3.Title); Assert.Null(b3.Comments); }
                );
            }
        }

        [Fact]
        public void DoesNotIncludeRelatedEntities_WithFetchAllExceptNavigationsStrategy()
        {
            var options = new DbContextOptionsBuilder<BloggingContext>().UseInMemoryDatabase(databaseName: "GetManyQueryHandler_DoesNotIncludeRelatedEntities_WhenFetchStrategyNotProvided").Options;
            var context = new BloggingContext(options);
            using (context)
            {
                var firstBlog = new Blog
                {
                    Id = 1,
                    Title = "blog title",
                    Comments = new List<Comment>() { new Comment { Content = "first comment" } , new Comment { Content = "second comment" } }
                };

                var secondBlog = new Blog
                {
                    Id = 2,
                    Title = "blog title",
                    Comments = new List<Comment>() { new Comment { Content = "third comment" } }
                };
            
                context.Blogs.AddRange(firstBlog, secondBlog);
                context.SaveChanges();

                var getAllBlogsQuery = new GetManyQuery<Blog>();
                var queryHandler = new GetManyQueryHandler<Blog>(new EfDataSourceBased(context), new FetchAllExceptNavigationsStrategy<Blog>(context));
                var blogs = queryHandler.Request(getAllBlogsQuery);

                Assert.NotNull(blogs);
                Assert.Equal(2, blogs.Count());
                Assert.All(blogs, blog =>
                {
                    Assert.NotNull(blog);
                    Assert.True(blog.Comments == null || context.Entry(blog).Collection(b => b.Comments).IsLoaded == false);
                });
            }
        }

        [Fact]
        public void IncludesRelatedEntities_WhenFetchProvided()
        {
            var options = new DbContextOptionsBuilder<BloggingContext>().UseInMemoryDatabase(databaseName: "GetManyQueryHandler_IncludesRelatedEntities_WhenFetchProvided").Options;
            var context = new BloggingContext(options);
            using (context)
            {
                var firstBlog = new Blog
                {
                    Id = 1,
                    Title = "blog title",
                    Comments = new List<Comment>() { new Comment { Content = "first comment" }, new Comment { Content = "second comment" } }
                };

                var secondBlog = new Blog
                {
                    Id = 2,
                    Title = "blog title",
                    Comments = new List<Comment>() { new Comment { Content = "third comment" } }
                };

                context.Blogs.AddRange(firstBlog, secondBlog);
                context.SaveChanges();

                var includeIdAndComments = new FetchOnlyStrategy<Blog>(b => b.Id, b => b.Comments);
                var getAllBlogsQuery = new GetManyQuery<Blog>(includeIdAndComments);
                var queryHandler = new GetManyQueryHandler<Blog>(new EfDataSourceBased(context), new FetchAllStrategy<Blog>());
                var blogs = queryHandler.Request(getAllBlogsQuery);

                Assert.NotNull(blogs);
                Assert.Equal(2, blogs.Count());
                Assert.All(blogs, blog =>
                {
                    Assert.NotNull(blog);
                    Assert.Null(blog.Title);
                    Assert.NotNull(blog.Comments);
                    Assert.NotEmpty(blog.Comments);
                });
            }
        }

        [Fact]
        public void AppliesPageSize_WhenProvided()
        {
            var options = new DbContextOptionsBuilder<BloggingContext>().UseInMemoryDatabase(databaseName: "GetManyQueryHandler_AppliesPageSize_WhenProvided").Options;
            var context = new BloggingContext(options);
            using (context)
            {
                var firstBlog = new Blog { Id = 1, Title = "blog title" };
                var secondBlog = new Blog { Id = 2, Title = "blog title" };
                var thirdBlog = new Blog { Id = 3, Title = "blog title" };
                context.Blogs.AddRange(firstBlog, secondBlog, thirdBlog);
                context.SaveChanges();

                var page = new Page(pageSize: 2);
                var query = new GetManyQuery<Blog>(page);
                var queryHandler = new GetManyQueryHandler<Blog>(new EfDataSourceBased(context), new FetchAllStrategy<Blog>());
                var blogs = (queryHandler as IQueryHandler<GetManyQuery<Blog>, ILimitedEnumerable<Blog>>).Request(query);

                Assert.NotNull(blogs);
                Assert.Equal(3, blogs.TotalCount);
                Assert.Equal(page.Size, blogs.Count());
                Assert.Collection(blogs,
                    b1 => { Assert.Equal(b1.Id, firstBlog.Id); },
                    b2 => { Assert.Equal(b2.Id, secondBlog.Id); }
                );
            }
        }

        [Fact]
        public void AppliesPageNumber_WhenProvided()
        {
            var options = new DbContextOptionsBuilder<BloggingContext>().UseInMemoryDatabase(databaseName: "GetManyQueryHandler_AppliesPageNumber_WhenProvided").Options;
            var context = new BloggingContext(options);
            using (context)
            {
                var firstBlog = new Blog { Id = 1, Title = "blog title" };
                var secondBlog = new Blog { Id = 2, Title = "blog title" };
                var thirdBlog = new Blog { Id = 3, Title = "blog title" };
                context.Blogs.AddRange(firstBlog, secondBlog, thirdBlog);
                context.SaveChanges();

                var page = new Page(pageNumber: 2, pageSize: 2);
                var query = new GetManyQuery<Blog>(page);
                var queryHandler = new GetManyQueryHandler<Blog>(new EfDataSourceBased(context), new FetchAllStrategy<Blog>());
                var blogs = (queryHandler as IQueryHandler<GetManyQuery<Blog>, ILimitedEnumerable<Blog>>).Request(query);

                Assert.NotNull(blogs);
                Assert.Equal(3, blogs.TotalCount);
                Assert.Single(blogs);
                Assert.Collection(blogs,
                    b1 => { Assert.Equal(b1.Id, thirdBlog.Id); }
                );
            }
        }
    }
}

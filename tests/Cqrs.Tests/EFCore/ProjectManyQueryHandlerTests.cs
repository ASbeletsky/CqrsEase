namespace Cqrs.Tests.EFCore
{
    #region Using
    using AutoMapper;
    using Cqrs.Common.Queries;
    using Cqrs.Common.Queries.FetchStrategies;
    using Cqrs.Common.Queries.Pagination;
    using Cqrs.Common.Queries.Sorting;
    using Cqrs.Core.Abstractions;
    using Cqrs.EntityFrameworkCore;
    using Cqrs.EntityFrameworkCore.DataSource;
    using Cqrs.EntityFrameworkCore.QueryHandlers;
    using Cqrs.Tests.Model;
    using Microsoft.EntityFrameworkCore;
    using NSpecifications;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;
    #endregion

    public class ProjectManyQueryHandlerTests
    {
        [Fact]
        public void AppliesSpecification_WhenProvided()
        {
            var options = new DbContextOptionsBuilder<BloggingContext>().UseInMemoryDatabase(databaseName: "ProjectManyQueryHandler_AppliesSpecification_WhenProvided").Options;
            var context = new BloggingContext(options);
            var projector = new AutoMapperProjector(new MapperConfiguration(c => c.AddProfile(typeof(TestMappingsProfile))).CreateMapper());
            using (context)
            {
                context.Blogs.AddRange(new Blog { Title = "first blog" }, new Blog { Title = "cool blog" }, new Blog { Title = "cool blog" });
                context.SaveChanges();
                var spec = new Spec<BlogDto>(b => b.Title == "cool blog");
                var query = new ProjectManyQuery<Blog, BlogDto>(spec);
                var queryHandler = new ProjectManyQueryHandler<Blog, BlogDto>(new EfDataSourceBased(context), projector);
                var blogsDtos = queryHandler.Request(query);

                Assert.NotNull(blogsDtos);
                Assert.Equal(2, blogsDtos.Count());
                Assert.All(blogsDtos, b =>
                {
                    Assert.NotNull(b);
                    Assert.True(spec.IsSatisfiedBy(b));
                });
            }
        }

        [Fact]
        public void AppliesSorting_WhenProvided()
        {
            var options = new DbContextOptionsBuilder<BloggingContext>().UseInMemoryDatabase(databaseName: "ProjectManyQueryHandler_AppliesSorting_WhenProvided").Options;
            var context = new BloggingContext(options);
            var projector = new AutoMapperProjector(new MapperConfiguration(c => c.AddProfile(typeof(TestMappingsProfile))).CreateMapper());
            using (context)
            {
                var firstBlog = new Blog { Id = 1, Title = "blog title" };
                var secondBlog = new Blog { Id = 2, Title = "blog title" };
                var thirdBlog = new Blog { Id = 3, Title = "blog title" };
                context.Blogs.AddRange(firstBlog, secondBlog, thirdBlog);
                context.SaveChanges();

                var orderByIdDesc = new OrderCreteria<BlogDto>(nameof(Blog.Id), OrderDirection.DESC);
                var query = new ProjectManyQuery<Blog, BlogDto>(orderByIdDesc);
                var queryHandler = new ProjectManyQueryHandler<Blog, BlogDto>(new EfDataSourceBased(context), projector);
                var blogsDtos = queryHandler.Request(query);

                Assert.NotNull(blogsDtos);
                Assert.Equal(3, blogsDtos.Count());
                Assert.Collection(blogsDtos,
                    b1 => Assert.Equal(b1.Id, thirdBlog.Id.ToString()),
                    b2 => Assert.Equal(b2.Id, secondBlog.Id.ToString()),
                    b3 => Assert.Equal(b3.Id, firstBlog.Id.ToString())
                );
            }
        }

        [Fact]
        public void SelectsAllFileds_WhenFetchStrategyNotProvided()
        {
            var options = new DbContextOptionsBuilder<BloggingContext>().UseInMemoryDatabase(databaseName: "ProjectManyQueryHandler_SelectsAllFileds_WhenFetchStrategyNotProvided").Options;
            var context = new BloggingContext(options);
            var projector = new AutoMapperProjector(new MapperConfiguration(c => c.AddProfile(typeof(TestMappingsProfile))).CreateMapper());
            using (context)
            {
                var firstBlog = new Blog { Id = 1, Title = "blog title" };
                var secondBlog = new Blog { Id = 2, Title = "blog title" };
                var thirdBlog = new Blog { Id = 3, Title = "blog title" };
                context.Blogs.AddRange(firstBlog, secondBlog, thirdBlog);
                context.SaveChanges();

                var query = new ProjectManyQuery<Blog, BlogDto>();
                var queryHandler = new ProjectManyQueryHandler<Blog, BlogDto>(new EfDataSourceBased(context), projector);
                var blogs = queryHandler.Request(query);

                Assert.NotNull(blogs);
                Assert.Equal(3, blogs.Count());
                Assert.Collection(blogs,
                    b1 => { Assert.Equal(b1.Id, firstBlog.Id.ToString()); Assert.Equal(b1.Title, firstBlog.Title); },
                    b2 => { Assert.Equal(b2.Id, secondBlog.Id.ToString()); Assert.Equal(b2.Title, secondBlog.Title); },
                    b3 => { Assert.Equal(b3.Id, thirdBlog.Id.ToString()); Assert.Equal(b3.Title, thirdBlog.Title); }
                );
            }
        }

        [Fact]
        public void SelectsOnlyNeededFileds_WhenFetchStrategyProvided()
        {
            var options = new DbContextOptionsBuilder<BloggingContext>().UseInMemoryDatabase(databaseName: "ProjectManyQueryHandler_SelectsOnlyNeededFileds_WhenFetchStrategyProvided").Options;
            var context = new BloggingContext(options);
            var projector = new AutoMapperProjector(new MapperConfiguration(c => c.AddProfile(typeof(TestMappingsProfile))).CreateMapper());
            using (context)
            {
                var firstBlog = new Blog { Id = 1, Title = "blog title" };
                var secondBlog = new Blog { Id = 2, Title = "blog title" };
                var thirdBlog = new Blog { Id = 3, Title = "blog title" };
                context.Blogs.AddRange(firstBlog, secondBlog, thirdBlog);
                context.SaveChanges();

                var includeOnlyId = new FetchOnlyStrategy<BlogDto>((b => b.Id));
                var query = new ProjectManyQuery<Blog, BlogDto>(includeOnlyId);
                var queryHandler = new ProjectManyQueryHandler<Blog, BlogDto>(new EfDataSourceBased(context), projector);
                var blogsDtos = queryHandler.Request(query);

                Assert.NotNull(blogsDtos);
                Assert.Equal(3, blogsDtos.Count());
                Assert.Collection(blogsDtos,
                    b1 => { Assert.Equal(b1.Id, firstBlog.Id.ToString()); Assert.Null(b1.Title); Assert.Null(b1.Comments); },
                    b2 => { Assert.Equal(b2.Id, secondBlog.Id.ToString()); Assert.Null(b2.Title); Assert.Null(b2.Comments); },
                    b3 => { Assert.Equal(b3.Id, thirdBlog.Id.ToString()); Assert.Null(b3.Title); Assert.Null(b3.Comments); }
                );
            }
        }

        [Fact]
        public void DoesNotIncludeRelatedEntities_WhenFetchStrategyNotProvided()
        {
            var options = new DbContextOptionsBuilder<BloggingContext>().UseInMemoryDatabase(databaseName: "ProjectManyQueryHandler_DoesNotIncludeRelatedEntities_WhenFetchStrategyNotProvided").Options;
            var context = new BloggingContext(options);
            var projector = new AutoMapperProjector(new MapperConfiguration(c => c.AddProfile(typeof(TestMappingsProfile))).CreateMapper());
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

                var getAllBlogsQuery = new ProjectManyQuery<Blog, BlogDto>();
                var queryHandler = new ProjectManyQueryHandler<Blog, BlogDto>(new EfDataSourceBased(context), projector);
                var blogsDtos = queryHandler.Request(getAllBlogsQuery);

                Assert.NotNull(blogsDtos);
                Assert.Equal(2, blogsDtos.Count());
                Assert.Collection(blogsDtos,
                    blog1 => { Assert.NotNull(blog1); Assert.True(blog1.Comments == null || context.Entry(firstBlog).Collection(b => b.Comments).IsLoaded == false); },
                    blog2 => { Assert.NotNull(blog2); Assert.True(blog2.Comments == null || context.Entry(secondBlog).Collection(b => b.Comments).IsLoaded == false); }
                );
            }
        }

        [Fact]
        public void IncludesRelatedEntities_WhenFetchProvided()
        {
            var options = new DbContextOptionsBuilder<BloggingContext>().UseInMemoryDatabase(databaseName: "ProjectManyQueryHandler_IncludesRelatedEntities_WhenFetchProvided").Options;
            var context = new BloggingContext(options);
            var mapper = new MapperConfiguration(c => c.AddProfile(typeof(TestMappingsProfile))).CreateMapper();
            var projector = new AutoMapperProjector(mapper);
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

                var includeIdAndComments = new FetchOnlyStrategy<BlogDto>(b => b.Id, b => b.Comments);
                var getAllBlogsQuery = new ProjectManyQuery<Blog, BlogDto>(includeIdAndComments);
                var queryHandler = new ProjectManyQueryHandler<Blog, BlogDto>(new EfDataSourceBased(context), projector);
                var blogsDtos = queryHandler.Request(getAllBlogsQuery);

                Assert.NotNull(blogsDtos);
                Assert.Equal(2, blogsDtos.Count());
                Assert.All(blogsDtos, blog =>
                {
                    Assert.NotNull(blog);
                    Assert.Null(blog.Title);
                    Assert.NotNull(blog.Comments);
                    Assert.NotEmpty(blog.Comments);
                });

                Assert.Collection(blogsDtos,
                    b1 => Assert.Equal(firstBlog.Comments.Select(c => c.Content), b1.Comments),
                    b2 => Assert.Equal(secondBlog.Comments.Select(c => c.Content), b2.Comments)
                );
            }
        }

        [Fact]
        public void AppliesPageSize_WhenProvided()
        {
            var options = new DbContextOptionsBuilder<BloggingContext>().UseInMemoryDatabase(databaseName: "ProjectManyQueryHandler_AppliesPageSize_WhenProvided").Options;
            var context = new BloggingContext(options);
            var mapper = new MapperConfiguration(c => c.AddProfile(typeof(TestMappingsProfile))).CreateMapper();
            var projector = new AutoMapperProjector(mapper);
            using (context)
            {
                var firstBlog = new Blog { Id = 1, Title = "blog title" };
                var secondBlog = new Blog { Id = 2, Title = "blog title" };
                var thirdBlog = new Blog { Id = 3, Title = "blog title" };
                context.Blogs.AddRange(firstBlog, secondBlog, thirdBlog);
                context.SaveChanges();

                var page = new Page(pageSize: 2);
                var query = new ProjectManyQuery<Blog, BlogDto>(page);
                var queryHandler = new ProjectManyQueryHandler<Blog, BlogDto>(new EfDataSourceBased(context), projector);
                var blogs = (queryHandler as IQueryHandler<ProjectManyQuery<Blog, BlogDto>, ILimitedEnumerable<BlogDto>>).Request(query);

                Assert.NotNull(blogs);
                Assert.Equal(3, blogs.TotalCount);
                Assert.Equal(page.Size, blogs.Count());
                Assert.Collection(blogs,
                    b1 => { Assert.Equal(b1.Id, firstBlog.Id.ToString()); },
                    b2 => { Assert.Equal(b2.Id, secondBlog.Id.ToString()); }
                );
            }
        }

        [Fact]
        public void AppliesPageNumber_WhenProvided()
        {
            var options = new DbContextOptionsBuilder<BloggingContext>().UseInMemoryDatabase(databaseName: "ProjectManyQueryHandler_AppliesPageNumber_WhenProvided").Options;
            var context = new BloggingContext(options);
            var mapper = new MapperConfiguration(c => c.AddProfile(typeof(TestMappingsProfile))).CreateMapper();
            var projector = new AutoMapperProjector(mapper);
            using (context)
            {
                var firstBlog = new Blog { Id = 1, Title = "blog title" };
                var secondBlog = new Blog { Id = 2, Title = "blog title" };
                var thirdBlog = new Blog { Id = 3, Title = "blog title" };
                context.Blogs.AddRange(firstBlog, secondBlog, thirdBlog);
                context.SaveChanges();

                var page = new Page(pageNumber: 2, pageSize: 2);
                var query = new ProjectManyQuery<Blog, BlogDto>(page);
                var queryHandler = new ProjectManyQueryHandler<Blog, BlogDto>(new EfDataSourceBased(context), projector);
                var blogs = (queryHandler as IQueryHandler<ProjectManyQuery<Blog, BlogDto>, ILimitedEnumerable<BlogDto>>).Request(query);

                Assert.NotNull(blogs);
                Assert.Equal(3, blogs.TotalCount);
                Assert.Single(blogs);
                Assert.Collection(blogs,
                    b1 => { Assert.Equal(b1.Id, thirdBlog.Id.ToString()); }
                );
            }
        }
    }
}

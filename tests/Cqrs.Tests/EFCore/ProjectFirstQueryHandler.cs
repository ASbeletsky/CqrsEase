using AutoMapper;
using Cqrs.Common.Queries;
using Cqrs.Common.Queries.FetchStateries;
using Cqrs.Common.Queries.Sorting;
using Cqrs.EntityFrameworkCore;
using Cqrs.EntityFrameworkCore.DataSource;
using Cqrs.EntityFrameworkCore.QueryHandlers;
using Cqrs.Tests.Model;
using Microsoft.EntityFrameworkCore;
using NSpecifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Cqrs.Tests.EFCore
{
    public class ProjectFirstQueryHandler
    {
        [Fact]
        public void AppliesSpecification_WhenProvided()
        {
            var options = new DbContextOptionsBuilder<BloggingContext>().UseInMemoryDatabase(databaseName: "ProjectFirstQueryHandler_AppliesSpecification_WhenProvided").Options;
            var context = new BloggingContext(options);
            var projector = new AutoMapperProjector(new MapperConfiguration(c => c.AddProfile(typeof(TestMappingsProfile))).CreateMapper());
            using (context)
            {
                context.Blogs.AddRange(new Blog { Title = "first blog" }, new Blog { Title = "second blog" }, new Blog { Title = "third blog" });
                context.SaveChanges();

                var expectedBlogTitle = "second blog";
                var spec = new Spec<BlogDto>(b => b.Title == expectedBlogTitle);
                var query = new ProjectFirstQuery<Blog, BlogDto>(spec);
                var queryHandler = new ProjectFirstQueryHandler<Blog, BlogDto>(new EfDataSourceBased(context), projector);
                var blogDto = queryHandler.Request(query);

                Assert.NotNull(blogDto);
                Assert.Equal(expectedBlogTitle, blogDto.Title);
            }
        }

        [Fact]
        public void AppliesSorting_WhenProvided()
        {
            var options = new DbContextOptionsBuilder<BloggingContext>().UseInMemoryDatabase(databaseName: "ProjectFirstQueryHandler_AppliesSorting_WhenProvided_AsString").Options;
            var context = new BloggingContext(options);
            var projector = new AutoMapperProjector(new MapperConfiguration(c => c.AddProfile(typeof(TestMappingsProfile))).CreateMapper());
            using (context)
            {
                context.Blogs.AddRange(new Blog { Id = 3, Title = "same blog title" }, new Blog { Id = 2, Title = "same blog title" }, new Blog { Id = 1, Title = "same blog title" });
                context.SaveChanges();

                var expectedBlogId = "1";
                var query = new ProjectFirstQuery<Blog, BlogDto>(sortingParams: new OrderCreteria<BlogDto>(b => b.Id));
                var queryHandler = new ProjectFirstQueryHandler<Blog, BlogDto>(new EfDataSourceBased(context), projector);
                var blogDto = queryHandler.Request(query);

                Assert.NotNull(blogDto);
                Assert.Equal(expectedBlogId, blogDto.Id);
            }
        }

        [Fact]
        public void SelectsAllFileds_WhenFetchStrategyNotProvided()
        {
            var options = new DbContextOptionsBuilder<BloggingContext>().UseInMemoryDatabase(databaseName: "ProjectFirstQueryHandler_SelectsAllFileds_WhenFetchStrategyNotProvided").Options;
            var context = new BloggingContext(options);
            var projector = new AutoMapperProjector(new MapperConfiguration(c => c.AddProfile(typeof(TestMappingsProfile))).CreateMapper());
            using (context)
            {
                var expectedBlog = new Blog { Id = 3, Title = "some blog title" };
                context.Blogs.Add(expectedBlog);
                context.SaveChanges();

                var query = new ProjectFirstQuery<Blog, BlogDto>();
                var queryHandler = new ProjectFirstQueryHandler<Blog, BlogDto>(new EfDataSourceBased(context), projector);
                var blogDto = queryHandler.Request(query);

                Assert.NotNull(blogDto);
                Assert.Equal(expectedBlog.Id.ToString(), blogDto.Id);
                Assert.Equal(expectedBlog.Title, blogDto.Title);
            }
        }

        [Fact]
        public void SelectsOnlyNeededFileds_WhenFetchStrategyProvided()
        {
            var options = new DbContextOptionsBuilder<BloggingContext>().UseInMemoryDatabase(databaseName: "ProjectFirstQueryHandler_SelectsOnlyNeededFileds_WhenFetchStrategyProvided").Options;
            var context = new BloggingContext(options);
            var projector = new AutoMapperProjector(new MapperConfiguration(c => c.AddProfile(typeof(TestMappingsProfile))).CreateMapper());
            using (context)
            {
                var expectedBlog = new Blog { Id = 3, Title = "some blog title" };
                context.Blogs.Add(expectedBlog);
                context.SaveChanges();

                var includeOnlyId = new FetchOnlyStratery<BlogDto>((b => b.Id));
                var query = new ProjectFirstQuery<Blog, BlogDto>(includeOnlyId);
                var queryHandler = new ProjectFirstQueryHandler<Blog, BlogDto>(new EfDataSourceBased(context), projector);
                var blogDto = queryHandler.Request(query);

                Assert.NotNull(blogDto);
                Assert.Equal(expectedBlog.Id.ToString(), blogDto.Id);
                Assert.Null(blogDto.Title);
            }
        }

        [Fact]
        public void DoesNotIncludeRelatedEntities_WhenFetchStrategyNotProvided()
        {
            var options = new DbContextOptionsBuilder<BloggingContext>().UseInMemoryDatabase(databaseName: "ProjectFirstQueryHandler_DoesNotIncludeRelatedEntities_WhenFetchStrategyNotProvided").Options;
            var context = new BloggingContext(options);
            var projector = new AutoMapperProjector(new MapperConfiguration(c => c.AddProfile(typeof(TestMappingsProfile))).CreateMapper());
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

                var getFirstBlogQuery = new ProjectFirstQuery<Blog, BlogDto>();
                var queryHandler = new ProjectFirstQueryHandler<Blog, BlogDto>(new EfDataSourceBased(context), projector);
                var loadedBlog = queryHandler.Request(getFirstBlogQuery);

                Assert.NotNull(loadedBlog);
                Assert.True(loadedBlog.Comments == null || context.Entry(blog).Collection(b => b.Comments).IsLoaded == false);
            }
        }

        [Fact]
        public void IncludesRelatedEntities_WhenFetchProvided()
        {
            var options = new DbContextOptionsBuilder<BloggingContext>().UseInMemoryDatabase(databaseName: "ProjectFirstQueryHandler_IncludesRelatedEntities_WhenFetchProvided").Options;
            var context = new BloggingContext(options);
            var projector = new AutoMapperProjector(new MapperConfiguration(c => c.AddProfile(typeof(TestMappingsProfile))).CreateMapper());
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

                var includeIdAndComments = new FetchOnlyStratery<BlogDto>(b => b.Id, b => b.Comments);
                var getFirstBlogQuery = new ProjectFirstQuery<Blog, BlogDto>(includeIdAndComments, sortingParams: new OrderCreteria<BlogDto>(b => b.Id));
                var queryHandler = new ProjectFirstQueryHandler<Blog, BlogDto>(new EfDataSourceBased(context), projector);
                var loadedBlog = queryHandler.Request(getFirstBlogQuery);

                Assert.NotNull(loadedBlog);
                Assert.Equal(blog.Id.ToString(), loadedBlog.Id);
                Assert.Null(loadedBlog.Title);
                Assert.Equal(blogComments.Select(c => c.Content), loadedBlog.Comments);
            }
        }
    }
}

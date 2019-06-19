namespace CqrsEase.Tests.EFCore
{
    using AutoMapper;
    #region Using
    using CqrsEase.Common.Commands;
    using CqrsEase.EntityFrameworkCore.CommandHandlers;
    using CqrsEase.EntityFrameworkCore.DataSource;
    using CqrsEase.Tests.Model;
    using Microsoft.EntityFrameworkCore;
    using NSpecifications;
    using System.Linq;
    using Xunit;
    #endregion

    public class UpdateQueryHandlerTests
    {
        [Fact]
        public void UpdatesOne()
        {
            var options = new DbContextOptionsBuilder<BloggingContext>().UseInMemoryDatabase(databaseName: "UpdateQueryHandlerTests_UpdatesOne").Options;
            var context = new BloggingContext(options);
            using (context)
            {
                var author = new Author { Id = 1, Name = "new author" };
                context.Authors.Add(author);
                context.SaveChanges();

                var updatedAuthor = new Author { Name = "updated author" };
                var authorToUpdate = new Spec<Author>(a => a.Id == 1);
                var updateCommand = new UpdateCommand<Author>(authorToUpdate, updatedAuthor);
                var commandHandler = new UpdateCommandHandler<Author>(new EfDataSourceBased(context));
                commandHandler.Apply(updateCommand);

                Assert.Equal(1, context.Authors.Count());
                Assert.Equal(author.Name, updatedAuthor.Name);
            }
        }

        [Fact]
        public void UpdatesOne_FromAnonymous()
        {
            var options = new DbContextOptionsBuilder<BloggingContext>().UseInMemoryDatabase(databaseName: "UpdateQueryHandlerTests_UpdatesOneFromAnonymous").Options;
            var context = new BloggingContext(options);
            using (context)
            {
                var author = new Author { Id = 1, Name = "new author" };
                context.Authors.Add(author);
                context.SaveChanges();

                var updatedAuthor = new { Name = "updated author", NonAuthorsProperty = "property that doesn't exists in entity type" };
                var authorToUpdate = new Spec<Author>(a => a.Id == 1);
                var updateCommand = new UpdateCommand<Author, object>(authorToUpdate, updatedAuthor);
                var commandHandler = new UpdateCommandHandler<Author, object>(new EfDataSourceBased(context), mapper: null);
                commandHandler.Apply(updateCommand);

                Assert.Equal(1, context.Authors.Count());
                Assert.Equal(author.Name, updatedAuthor.Name);
            }
        }

        [Fact]
        public void UpdatesOne_FromDto()
        {
            var options = new DbContextOptionsBuilder<BloggingContext>().UseInMemoryDatabase(databaseName: "UpdateQueryHandlerTests_UpdatesOneFromDto").Options;
            var context = new BloggingContext(options);
            var mapper = new MapperConfiguration(c => c.AddProfile(typeof(TestMappingsProfile))).CreateMapper();
            using (context)
            {
                var blog = new Blog { Id = 1, Title = "new blog" };
                context.Blogs.Add(blog);
                context.SaveChanges();

                var updatedBlog = new BlogDto { Title = "updated blog"};
                var blogToUpdate = new Spec<Blog>(b => b.Id == 1);
                var updateCommand = new UpdateCommand<Blog, BlogDto>(blogToUpdate, updatedBlog);
                var commandHandler = new UpdateCommandHandler<Blog, BlogDto>(new EfDataSourceBased(context), mapper);
                commandHandler.Apply(updateCommand);

                Assert.Equal(1, context.Blogs.Count());
                Assert.Equal(blog.Title, updatedBlog.Title);
            }
        }
    }
}

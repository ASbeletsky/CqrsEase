namespace CqrsEase.Tests.EFCore
{
    #region Using
    using CqrsEase.Common.Commands;
    using CqrsEase.EntityFrameworkCore.CommandHandlers;
    using CqrsEase.EntityFrameworkCore.DataSource;
    using CqrsEase.Tests.Model;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using Xunit;

    #endregion

    public class CreateCommandHandlerTests
    {
        [Fact]
        public void CreatesOne()
        {
            var options = new DbContextOptionsBuilder<BloggingContext>().UseInMemoryDatabase(databaseName: "CreateCommandHandlerTests_CreatesNew").Options;
            var context = new BloggingContext(options);
            using (context)
            {
                var author = new Author { Name = "new author" };
                var createCommand = new CreateCommand<Author>(author);
                var commandHandler = new CreateCommandHandler<Author>(new EfDataSourceBased(context));
                commandHandler.Apply(createCommand);

                Assert.Equal(1, context.Authors.Count());
                Assert.Equal(context.Authors.First(), author);
            }
        }

        [Fact]
        public void CreatesRange()
        {
            var options = new DbContextOptionsBuilder<BloggingContext>().UseInMemoryDatabase(databaseName: "CreateCommandHandlerTests_CreatesRange").Options;
            var context = new BloggingContext(options);
            using (context)
            {
                var author1 = new Author { Name = "first author" };
                var author2 = new Author { Name = "second author" };
                var createCommand = new CreateManyCommand<Author>(author1, author2);
                var commandHandler = new CreateManyCommandHandler<Author>(new EfDataSourceBased(context));
                commandHandler.Apply(createCommand);

                Assert.Equal(2, context.Authors.Count());
                Assert.Equal(context.Authors.First(), author1);
                Assert.Equal(context.Authors.Last(), author2);
            }
        }
    }
}

namespace Cqrs.Tests.EFCore
{
    #region Using
    using Cqrs.Common.Commands;
    using Cqrs.EntityFrameworkCore.CommandHandlers;
    using Cqrs.EntityFrameworkCore.DataSource;
    using Cqrs.Tests.Model;
    using Microsoft.EntityFrameworkCore;
    using NSpecifications;
    using System.Linq;
    using Xunit;

    #endregion

    public class DeleteCommandHandlerTests
    {
        [Fact]
        public void DeletesOne()
        {
            var options = new DbContextOptionsBuilder<BloggingContext>().UseInMemoryDatabase(databaseName: "DeleteCommandHandlerTests_DeletesOne").Options;
            var context = new BloggingContext(options);
            using (context)
            {
                var author = new Author { Id = 1, Name = "new author" };
                context.Authors.Add(author);
                context.SaveChanges();

                var authorToDelete = new Spec<Author>(a => a.Id == 1);
                var createCommand = new DeleteCommand<Author>(authorToDelete);
                var commandHandler = new DeleteCommandHandler<Author>(new EfDataSourceBased(context));
                commandHandler.Apply(createCommand);

                Assert.Equal(0, context.Authors.Count());
                Assert.Equal(EntityState.Detached, context.Entry(author).State);
            }
        }
    }
}

using Cqrs.Common.Commands;
using Cqrs.EntityFrameworkCore.CommandHandlers;
using Cqrs.EntityFrameworkCore.DataSource;
using Cqrs.Tests.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Xunit;

namespace Cqrs.Tests.EFCore
{
    public class CreateCommandHandlerTests
    {
        [Fact]
        public void CreatesNew()
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
    }
}

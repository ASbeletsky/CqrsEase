using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.Common;

namespace Cqrs.Tests.EFCore
{
    class SqlLiteInMemoryContextFactory : IDisposable
    {
        private DbConnection _connection;

        private DbContextOptions<BloggingContext> CreateOptions()
        {
            return new DbContextOptionsBuilder<BloggingContext>()
                .UseSqlite(_connection).Options;
        }

        public BloggingContext CreateContext()
        {
            if (_connection == null)
            {
                _connection = new SqliteConnection("DataSource=:memory:");
                _connection.Open();

                var options = CreateOptions();
                using (var context = new BloggingContext(options))
                {
                    context.Database.EnsureCreated();
                }
            }

            return new BloggingContext(CreateOptions());
        }

        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Dispose();
                _connection = null;
            }
        }
    }
}

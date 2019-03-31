using Cqrs.EntityFrameworkCore.DataSource;
using Cqrs.Tests.Model;
using Microsoft.EntityFrameworkCore;
using NSpecifications;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Cqrs.Tests.EFCore
{
    public class EfDataSourceBasedTests
    {
        [Fact]
        public void UpdatesEntityProperties()
        {
            var options = new DbContextOptionsBuilder<BloggingContext>().UseInMemoryDatabase(databaseName: "EfDataSourceBasedTests_UpdatesEntityProperties").Options;
            var context = new BloggingContext(options);
            using (context)
            {
                var existingBlog = new Blog() { Id = 1, Title = "some title" };
                context.Blogs.Add(existingBlog);
                context.SaveChanges();

                var dataSource = new EfDataSourceBased(context);
                var updateFirstBlog = new Spec<Blog>(b => b.Id == 1);
                var updatedBlog = new Blog() { Title = "changed title" };
                dataSource.Update(updateFirstBlog, updatedBlog);
                existingBlog = context.Blogs.Find(1);

                Assert.NotNull(existingBlog);
                Assert.Equal(existingBlog.Title, updatedBlog.Title);
            }
        }

        [Fact]
        public void UpdatesEntityReferences()
        {
            var options = new DbContextOptionsBuilder<BloggingContext>().UseInMemoryDatabase(databaseName: "EfDataSourceBasedTests_UpdatesEntityReferences").Options;
            var context = new BloggingContext(options);
            using (context)
            {
                var existingAuthor = new Author { Name = "existing author" };
                var existingBlog = new Blog() { Id = 1, Title = "some title", Author = existingAuthor };
                context.Blogs.Add(existingBlog);
                context.SaveChanges();

                var dataSource = new EfDataSourceBased(context);
                var updateFirstBlog = new Spec<Blog>(b => b.Id == 1);
                var newAuthor = new Author { Name = "new author" };
                var updatedBlog = new Blog() { Author = newAuthor };
                dataSource.Update(updateFirstBlog, updatedBlog);
                existingBlog = context.Blogs.Find(1);

                Assert.NotNull(existingBlog);
                Assert.NotNull(existingBlog.Author);
                Assert.Equal(existingBlog.Author.Name, newAuthor.Name);
            }
        }
    }
}

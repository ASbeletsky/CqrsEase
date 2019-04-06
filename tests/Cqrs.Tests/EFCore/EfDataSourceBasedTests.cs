namespace Cqrs.Tests.EFCore
{
    #region Using
    using Cqrs.EntityFrameworkCore.DataSource;
    using Cqrs.Tests.Model;
    using Microsoft.EntityFrameworkCore;
    using NSpecifications;
    using Xunit;
    #endregion

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
                dataSource.UpdateRange(updateFirstBlog, updatedBlog);
                existingBlog = context.Blogs.Find(1);

                Assert.NotNull(existingBlog);
                Assert.Equal(existingBlog.Title, updatedBlog.Title);
            }
        }
    }
}

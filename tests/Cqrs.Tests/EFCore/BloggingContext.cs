using Cqrs.Tests.Model;
using Microsoft.EntityFrameworkCore;

namespace Cqrs.Tests.EFCore
{
    internal class BloggingContext : DbContext
    {
        public BloggingContext()
        {
        }

        public BloggingContext(DbContextOptions<BloggingContext> options)
            : base(options)
        {
        }   

        public DbSet<Blog> Blogs { get; set; }
    }
}

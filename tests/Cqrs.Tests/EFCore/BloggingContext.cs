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

        public DbSet<Author> Authors { get; set; }

        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Blog>().HasOne(b => b.Author).WithMany(a => a.Blogs).HasForeignKey(b => b.AuthorId);
            modelBuilder.Entity<Blog>().HasMany(b => b.Comments).WithOne().HasForeignKey(c => c.BlogId);
        }
    }
}

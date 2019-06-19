using System.Collections.Generic;

namespace CqrsEase.Tests.Model
{
    internal class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Blog> Blogs { get; set; }
    }
}

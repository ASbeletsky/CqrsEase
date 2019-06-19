using System.Collections.Generic;

namespace CqrsEase.Tests.Model
{
    internal class Blog
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
    }
}

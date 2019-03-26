namespace Cqrs.Tests.Model
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int BlogId { get; set; }
    }
}

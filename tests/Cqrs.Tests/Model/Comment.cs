namespace Cqrs.Tests.Model
{
    using Cqrs.JsonApi;

    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int BlogId { get; set; }
    }

    public class CommentDto : IResource
    {
        public string Id { get; set; }
        public string Content { get; set; }

        public string Type => "comment";
    }
}

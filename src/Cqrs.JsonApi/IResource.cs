namespace Cqrs.JsonApi
{
    public interface IResource
    {
        string Id { get; }

        string Type { get; }
    }
}

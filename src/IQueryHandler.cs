namespace Cqrs.Core
{
    public interface IQueryHandler<in TQuery, out TResult>
        where TQuery : IQuery<TResult>
    {
        TResult Query(TQuery query);
    }
}

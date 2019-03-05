namespace Cqrs.Core
{
    using System.Threading.Tasks;

    public interface IQueryHandlerAsync<in TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        Task<TResult> QueryAsync(TQuery query);
    }
}

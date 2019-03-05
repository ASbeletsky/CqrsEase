namespace Cqrs.Core.Abstractions
{
    using System.Threading.Tasks;

    public interface IQueryProcessor
    {
        TResult Execute<TResult>(IQuery<TResult> query);

        Task<TResult> ExecuteAsync<TResult>(IQuery<TResult> query);
    }
}

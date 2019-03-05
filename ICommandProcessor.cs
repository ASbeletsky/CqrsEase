namespace Cqrs.Core
{
    using System.Threading.Tasks;

    public interface ICommandProcessor
    {
        void Execute(ICommand command);

        TResult Execute<TResult>(ICommand<TResult> command);

        Task ExecuteAsync(ICommand command);

        Task<TResult> ExecuteAsync<TResult>(ICommand<TResult> command);
    }
}

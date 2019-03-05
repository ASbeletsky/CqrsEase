namespace Cqrs.Core
{
    using System.Threading.Tasks;

    public interface ICommandHandlerAsync<in TCommand>
        where TCommand : ICommand
    {
        Task ExecuteAsync(TCommand command);
    }

    public interface ICommandHandlerAsync<in TCommand, TResult>
        where TCommand : ICommand<TResult>
    {
        Task<TResult> ExecuteAsync(TCommand command);
    }
}

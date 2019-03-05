namespace Cqrs.Core.Abstractions
{
    public interface ICommand
    {
    }

    public interface ICommand<out TResult>
    {
    }
}

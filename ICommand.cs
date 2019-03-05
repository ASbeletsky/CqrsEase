namespace Cqrs.Core
{
    public interface ICommand
    {
    }

    public interface ICommand<out TResult>
    {
    }
}

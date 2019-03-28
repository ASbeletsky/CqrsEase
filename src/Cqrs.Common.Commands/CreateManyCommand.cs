namespace Cqrs.Common.Commands
{
    #region Using
    using Cqrs.Core.Abstractions;
    using System.Collections.Generic;
    #endregion

    public class CreateManyCommand<T> : ICommand
    {
        public CreateManyCommand(params T[] valuesParams) : this(values: valuesParams)
        {
        }

        public CreateManyCommand(IEnumerable<T> values)
        {
            Values = values;
        }

        public IEnumerable<T> Values { get; }
    }

    public class CreateManyCommand<T, TResult> : CreateManyCommand<T>, ICommand<TResult>
    {
        public CreateManyCommand(params T[] valuesParams) : base(valuesParams)
        {
        }

        public CreateManyCommand(IEnumerable<T> values) : base(values)
        {
        }
    }
}

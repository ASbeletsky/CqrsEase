namespace Cqrs.Common.Queries.Sorting
{
    #region Using
    using System;
    using System.Linq.Expressions;
    #endregion

    public class OrderCreteria<T>
    {
        public OrderCreteria(string sortKey, OrderDirection direction = OrderDirection.ASC)
        {
            SortKey = sortKey;
            Direction = direction;
        }

        public OrderCreteria(Expression<Func<T, object>> sortKeySelector, OrderDirection direction = OrderDirection.ASC)
            : this(sortKeySelector.ToPropertyName(), direction)
        {
        }

        public string SortKey { get; }
        public OrderDirection Direction { get; }
    }
}

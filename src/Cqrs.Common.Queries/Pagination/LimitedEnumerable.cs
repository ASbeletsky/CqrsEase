namespace Cqrs.Common.Queries.Pagination
{
    #region
    using System.Collections;
    using System.Collections.Generic;
    #endregion

    public class LimitedEnumerable<T> : ILimitedEnumerable<T>
    {
        private readonly IEnumerable<T> _inner;
        private readonly int _totalCount;

        public LimitedEnumerable(IEnumerable<T> inner, int totalCount)
        {
            _inner = inner;
            _totalCount = totalCount;
        }

        public int TotalCount => _totalCount;

        public IEnumerator<T> GetEnumerator()
        {
            return _inner.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

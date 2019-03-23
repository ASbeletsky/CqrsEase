using System;
using System.Collections.Generic;
using System.Text;

namespace Cqrs.Common.Queries.Pagination
{
    public class Page : IPage
    {
        public const int DefaultPageSize = 10;

        public const int DefaultPageNumber = 1;

        public Page(int pageNumber = DefaultPageNumber, int pageSize = DefaultPageSize)
        {
            Number = pageNumber;
            Size = pageSize;
        }

        public int Number { get; }

        public int Size { get; }

        public int Offset => (Number - 1) * Size;

        public Page Next() => new Page(Number + 1, Size);

        public Page Previous() => this.Number >= 2 ? new Page(Number - 1, Size) : null;

        public static Page First(int pageSize) => new Page(1, pageSize);

        public static Page Last(int totalCount, int pageSize)
        {
            var lastPageNumber = (int)Math.Ceiling((decimal)totalCount / pageSize);
            var lastPage = new Page(lastPageNumber, pageSize);
            return lastPage;
        }

        public static bool HasNext(Page page, int totalCount) => page.Offset + page.Size < totalCount;

        public static bool HasPrevious(Page page) => page.Offset - page.Size >= 0;
    }
}

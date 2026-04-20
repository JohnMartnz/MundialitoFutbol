using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Application.Common
{
    public record PagedResult<T>(
        IEnumerable<T> Items,
        int TotalCount,
        int PageNumber,
        int PageSize
    )
    {
        public int TotalPage => (int)Math.Ceiling(TotalCount / (double)PageSize);
        public bool HasNextPage => PageNumber < TotalPage;
        public bool HasPreviousPage => PageNumber > 1;
    }
}

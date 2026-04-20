using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Application.Common
{
    public record PagedResult<T>(
        IEnumerable<T> Data,
        int TotalRecords,
        int PageNumber,
        int PageSize
    )
    {
        public int TotalPages => (int)Math.Ceiling(TotalRecords/ (double)PageSize);
        public bool HasNextPage => PageNumber < TotalPages;
        public bool HasPreviousPage => PageNumber > 1;
    }
}

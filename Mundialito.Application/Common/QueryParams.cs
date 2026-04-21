using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Application.Common
{
    public record QueryParams(
        int PageNumber = 1,
        int PageSize = 10,
        string? Search = null,
        string? SortBy = null,
        string? SortDirection = "ASC"
    );
}

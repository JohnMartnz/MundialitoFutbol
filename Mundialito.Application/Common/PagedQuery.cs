using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Application.Common
{
    public record PagedQuery(int PageNumber = 1, int PageSize = 10);
}

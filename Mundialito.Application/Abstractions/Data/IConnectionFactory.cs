using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Mundialito.Application.Abstractions.Data
{
    public interface IConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}

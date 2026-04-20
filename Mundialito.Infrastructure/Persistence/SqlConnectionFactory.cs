using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Mundialito.Application.Abstractions.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Mundialito.Infrastructure.Persistence
{
    public class SqlConnectionFactory : IConnectionFactory
    {
        private readonly string _connectionString;
        public SqlConnectionFactory(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}

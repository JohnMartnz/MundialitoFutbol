using Dapper;
using Microsoft.Data.SqlClient;
using Mundialito.Application.Abstractions.Data;
using Mundialito.Application.Abstractions.Repositories;
using Mundialito.Application.Common;
using Mundialito.Application.Features.Tournaments.Queries.GetTournaments;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Mundialito.Infrastructure.Repositories
{
    public class TournamentQueryRepository : ITournamentQueryRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        public TournamentQueryRepository(IConnectionFactory sqlConnectionFactory)
        {
            _connectionFactory = sqlConnectionFactory;
        }

        public async Task<PagedResult<TournamentDto>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? search,
            string? sortBy,
            string? sortDirection,
            CancellationToken cancellationToken = default
        )
        {
            using var connection = _connectionFactory.CreateConnection();

            var where = new StringBuilder("WHERE 1 = 1");

            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(search))
            {
                where.Append(" AND Name LIKE @Search ");
                parameters.Add("Search", $"%{search}%");
            }

            var orderBy = sortBy?.ToLower() switch
            {
                "name" => "Name",
                "startdate" => "StartDate",
                "enddate" => "EndDate",
                _ => "Name"
            };

            var direction = sortDirection?.ToUpper() == "DESC" ? "DESC" : "ASC";

            var offset = Math.Max(0, (pageNumber - 1) * pageSize);

            var sql = $@"
                SELECT Id, Name, StartDate, EndDate
                FROM Tournaments
                {where}
                ORDER BY {orderBy} {direction}
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

                SELECT COUNT(*)
                FROM Tournaments
                {where};
            ";

            parameters.Add("Offset", offset);
            parameters.Add("PageSize", pageSize);

            using var multi = await connection.QueryMultipleAsync(
                new CommandDefinition(sql, parameters, cancellationToken: cancellationToken)
            );

            var data = await multi.ReadAsync<TournamentDto>();
            var totalRecords = await multi.ReadFirstAsync<int>();

            return new PagedResult<TournamentDto>(data, totalRecords, pageNumber, pageSize);
        }
    }
}

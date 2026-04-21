using Azure.Core;
using Dapper;
using Mundialito.Application.Abstractions.Data;
using Mundialito.Application.Abstractions.Repositories;
using Mundialito.Application.Common;
using Mundialito.Application.Features.Teams.Queries.GetTeams;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Infrastructure.Repositories
{
    public class TeamQueryRepository : ITeamQueryRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        public TeamQueryRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<PagedResult<TeamsResponse>> GetPagedAsync(
            QueryParams queryParams,
            CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.CreateConnection();

            var whereClause = new StringBuilder("WHERE 1 = 1");
            var parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(queryParams.Search))
            {
                whereClause.Append(" AND Name LIKE @Search");
                parameters.Add("Search", $"%{queryParams.Search}%");
            }

            var orderByClause = queryParams.SortBy?.ToLower() switch
            {
                "name" => "Name",
                _ => "Name"
            };

            var sortDirectionClause = queryParams.SortDirection?.ToLower() == "desc" ? "DESC" : "ASC";
            var offset = (queryParams.PageNumber - 1) * queryParams.PageSize;

            var sql = $@"
                SELECT Id, Name
                FROM Teams
                {whereClause}
                ORDER BY {orderByClause} {sortDirectionClause}
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

                SELECT COUNT(1) FROM Teams {whereClause};
            ";

            parameters.Add("Offset", offset);
            parameters.Add("PageSize", queryParams.PageSize);

            using var multi = await connection.QueryMultipleAsync(new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));
            var items = (await multi.ReadAsync<TeamsResponse>()).ToList();
            var totalCount = await multi.ReadFirstAsync<int>();

            return new PagedResult<TeamsResponse>(items, totalCount, queryParams.PageNumber, queryParams.PageSize);
        }
    }
}

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
            CancellationToken cancellationToken
        )
        {
            using var connection = _connectionFactory.CreateConnection();
            var sortColumns = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "name", "Name" },
                { "id", "Id" }
            };

            var sortBy = sortColumns.TryGetValue(queryParams.SortBy ?? "name", out var column)
                ? column
                : "Name";

            var sortDirection = queryParams.SortDirection?.ToLower() == "desc"
                ? "DESC"
                : "ASC";

            var conditions = new List<string>();
            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(queryParams.Search))
            {
                conditions.Add("Name LIKE @Search");
                parameters.Add("Search", $"%{queryParams.Search}%");
            }

            var whereClause = conditions.Count > 0
                ? "WHERE " + string.Join(" AND ", conditions)
                : string.Empty;

            var pageNumber = queryParams.PageNumber <= 0 ? 1 : queryParams.PageNumber;
            var pageSize = queryParams.PageSize <= 0 ? 10 : queryParams.PageSize;
            var offset = (pageNumber - 1) * pageSize;

            parameters.Add("Offset", offset);
            parameters.Add("PageSize", pageSize);

            var sql = $@"
                SELECT Id, Name
                FROM Teams
                {whereClause}
                ORDER BY {sortBy} {sortDirection}
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

                SELECT COUNT(1)
                FROM Teams
                {whereClause};
            ";

            using var multi = await connection.QueryMultipleAsync(
                new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));

            var items = (await multi.ReadAsync<TeamsResponse>()).ToList();
            var totalCount = await multi.ReadFirstAsync<int>();

            return new PagedResult<TeamsResponse>(
                items,
                totalCount,
                pageNumber,
                pageSize
            );
        }
    }
}

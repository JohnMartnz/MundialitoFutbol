using Dapper;
using Mundialito.Application.Abstractions.Data;
using Mundialito.Application.Abstractions.Repositories;
using Mundialito.Application.Common;
using Mundialito.Application.Features.Players.Queries.GetPlayers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Infrastructure.Repositories
{
    public class PlayerQueryRepository : IPlayerQueryRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        public PlayerQueryRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<PagedResult<PlayersResponse>> GetPlayersAsync(
            QueryParams queryParams,
            Guid? teamId,
            CancellationToken cancellationToken
        )
        {
            using var connection = _connectionFactory.CreateConnection();

            var sortColumns = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "name", "p.Name" },
                { "position", "p.Position" },
                { "team", "t.Name" },
                { "id", "p.Id" }
            };

            var sortBy = sortColumns.TryGetValue(queryParams.SortBy ?? "name", out var column)
                ? column
                : "p.Name";

            var sortDirection = queryParams.SortDirection?.ToLower() == "desc"
                ? "DESC"
                : "ASC";

            var conditions = new List<string>();
            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(queryParams.Search))
            {
                conditions.Add("p.Name LIKE @Search");
                parameters.Add("Search", $"%{queryParams.Search}%");
            }

            if (teamId.HasValue)
            {
                conditions.Add("p.TeamId = @TeamId");
                parameters.Add("TeamId", teamId.Value);
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
                SELECT p.Id, p.Name, p.Position, p.TeamId, t.Name AS TeamName
                FROM Players p
                INNER JOIN Teams t ON p.TeamId = t.Id
                {whereClause}
                ORDER BY {sortBy} {sortDirection}
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

                SELECT COUNT(1)
                FROM Players p
                INNER JOIN Teams t ON p.TeamId = t.Id
                {whereClause};
            ";

            parameters.Add("Offset", offset);
            parameters.Add("PageSize", pageSize);

            using var multi = await connection.QueryMultipleAsync(
                new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));

            var items = (await multi.ReadAsync<PlayersResponse>()).ToList();
            var totalCount = await multi.ReadFirstAsync<int>();

            return new PagedResult<PlayersResponse>(
                items,
                totalCount,
                pageNumber,
                pageSize
            );
        }
    }
}

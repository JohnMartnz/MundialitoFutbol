using Dapper;
using Mundialito.Application.Abstractions.Data;
using Mundialito.Application.Abstractions.Repositories;
using Mundialito.Application.Common;
using Mundialito.Application.Features.Matches.Queries.GetMatches;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Infrastructure.Repositories
{
    public class MatchQueryRepository : IMatchQueryRepository
    {
        private readonly IConnectionFactory _connectionFactory;
        
        public MatchQueryRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<PagedResult<MatchesResponse>> GetMatchesAsync(QueryParams queryParams, Guid? tournamentId, Guid? teamId, CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.CreateConnection();
            var sortColumns = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "date", "m.MatchDate" },
                { "homeTeam", "ht.Name" },
                { "visitingTeam", "vt.Name" },
                { "id", "m.Id" }
            };

            var sortBy = sortColumns.TryGetValue(queryParams.SortBy ?? "date", out var column) ? column : "m.MatchDate";
            var sortDirection = queryParams.SortDirection?.ToLower() == "desc" ? "DESC" : "ASC";

            var conditions = new List<string>();
            var parameters = new DynamicParameters();

            if(!string.IsNullOrWhiteSpace(queryParams.Search))
            {
                conditions.Add("(ht.Name ILIKE @Search OR vt.Name ILIKE @Search)");
                parameters.Add("Search", $"%{queryParams.Search}%");
            }

            if(tournamentId.HasValue)
            {
                conditions.Add("m.TournamentId = @TournamentId");
                parameters.Add("TournamentId", tournamentId.Value);
            }

            if(teamId.HasValue)
            {
                conditions.Add("(m.HomeTeamId = @TeamId OR m.VisitingTeamId = @TeamId)");
                parameters.Add("TeamId", teamId.Value);
            }

            var whereClause = conditions.Count > 0 ? $"WHERE {string.Join(" AND ", conditions)}" : string.Empty;

            var pageNumber = queryParams.PageNumber > 0 ? queryParams.PageNumber : 1;
            var pageSize = queryParams.PageSize > 0 ? queryParams.PageSize : 10;
            var offset = (pageNumber - 1) * pageSize;

            parameters.Add("Offset", offset);
            parameters.Add("PageSize", pageSize);

            var sql = $@"
                SELECT 
                    m.Id,
                    m.TournamentId,
                    m.HomeTeamId,
                    m.VisitingTeamId,
                    m.HomeTeamGoals,
                    m.VisitingTeamGoals,
                    m.MatchDate,
                    m.Status,
                    ht.Name AS HomeTeamName,
                    vt.Name AS VisitingTeamName
                FROM Matches m
                INNER JOIN Teams ht ON m.HomeTeamId = ht.Id
                INNER JOIN Teams vt ON m.VisitingTeamId = vt.Id
                {whereClause}
                ORDER BY {sortBy} {sortDirection}
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

                SELECT COUNT(1)
                FROM Matches m
                INNER JOIN Teams ht ON m.HomeTeamId = ht.Id
                INNER JOIN Teams vt ON m.VisitingTeamId = vt.Id
                {whereClause};
            ";

            using var multi = await connection.QueryMultipleAsync(new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));
            var items = (await multi.ReadAsync<MatchesResponse>()).ToList();
            var totalRecords = await multi.ReadFirstAsync<int>();

            return new PagedResult<MatchesResponse>(items, totalRecords, pageNumber, pageSize);
        }
    }
}

using Dapper;
using Microsoft.Data.SqlClient;
using Mundialito.Application.Abstractions.Data;
using Mundialito.Application.Abstractions.Repositories;
using Mundialito.Application.Common;
using Mundialito.Application.Features.Tournaments.Queries.GetTournaments;
using System;
using System.Collections.Generic;
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

        public async Task<PagedResult<TournamentDto>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sqlItems = @"
                SELECT Id, Name, StartDate, EndDate 
                FROM Tournaments 
                ORDER BY Name 
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
                
                SELECT COUNT(*) FROM Tournaments;
            ";

            var offset = Math.Max(0, (pageNumber - 1) * pageSize);

            using var multi = await connection.QueryMultipleAsync(new CommandDefinition(sqlItems, new { Offset = offset, PageSize = pageSize }, cancellationToken: cancellationToken));
            var items = await multi.ReadAsync<TournamentDto>();
            var totalCount = await multi.ReadSingleAsync<int>();

            return new PagedResult<TournamentDto>(items, totalCount, pageNumber, pageSize);
        }
    }
}

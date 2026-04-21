using Mundialito.Application.Abstractions.Repositories;
using Mundialito.Domain.Entities;
using Mundialito.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Infrastructure.Repositories
{
    public class MatchRepository : IMatchRepository
    {
        private readonly MundialitoDbContext _dbContext;

        public MatchRepository(MundialitoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Match match, CancellationToken cancellationToken)
        {
            await _dbContext.Matches.AddAsync(match, cancellationToken);
        }

        public async Task<Match?> GetByIdAsync(Guid matchId,  CancellationToken cancellationToken)
        {
            return await _dbContext.Matches.FindAsync(new object[] { matchId }, cancellationToken);
        }
    }
}

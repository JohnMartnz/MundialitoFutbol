using Mundialito.Application.Abstractions.Repositories;
using Mundialito.Domain.Entities;
using Mundialito.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Infrastructure.Repositories
{
    public class GoalMatchRepository : IGoalMatchRepository
    {
        private readonly MundialitoDbContext _dbContext;

        public GoalMatchRepository(MundialitoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(GoalMatch goal, CancellationToken cancellationToken)
        {
            await _dbContext.GoalMatches.AddAsync(goal, cancellationToken);
        }
    }
}

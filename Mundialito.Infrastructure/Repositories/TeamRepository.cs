using Microsoft.EntityFrameworkCore;
using Mundialito.Application.Abstractions.Repositories;
using Mundialito.Domain.Entities;
using Mundialito.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Infrastructure.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly MundialitoDbContext _dbContext;

        public TeamRepository(MundialitoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Team team, CancellationToken cancellationToken = default)
        {
            await _dbContext.Teams.AddAsync(team, cancellationToken);
        }

        public async Task<Team?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Teams.FirstOrDefaultAsync(team => team.Name == name, cancellationToken);
        }

        public async Task<Team?> GetTeamByIdAsync(Guid teamId, CancellationToken cancellationToken)
        {
            return await _dbContext.Teams.FindAsync(new object[] { teamId }, cancellationToken);
        }
    }
}

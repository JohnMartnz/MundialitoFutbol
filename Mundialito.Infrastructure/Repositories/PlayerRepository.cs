using Mundialito.Application.Abstractions.Repositories;
using Mundialito.Domain.Entities;
using Mundialito.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Infrastructure.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly MundialitoDbContext _dbContext;

        public PlayerRepository(MundialitoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Player player, CancellationToken cancellationToken)
        {
            await _dbContext.Players.AddAsync(player, cancellationToken);
        }
    }
}

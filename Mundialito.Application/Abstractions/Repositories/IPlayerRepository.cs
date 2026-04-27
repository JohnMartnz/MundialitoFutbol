using Mundialito.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Application.Abstractions.Repositories
{
    public interface IPlayerRepository
    {
        Task AddAsync(Player player, CancellationToken cancellationToken);
        Task<Player?> GetByIdAsync(Guid playerId, CancellationToken cancellationToken);
    }
}

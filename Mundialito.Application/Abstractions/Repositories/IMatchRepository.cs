using Mundialito.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Application.Abstractions.Repositories
{
    public interface IMatchRepository
    {
        Task AddAsync(Match match, CancellationToken cancellationToken = default);
        Task<Match?> GetByIdAsync(Guid matchId, CancellationToken cancellationToken = default);
    }
}

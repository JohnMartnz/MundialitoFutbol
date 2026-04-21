using Mundialito.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Application.Abstractions.Repositories
{
    public interface ITournamentRepository
    {
        Task AddAsync(Tournament tournament, CancellationToken cancellationToken = default);
        Task<bool> NameExistsAsync(string name, CancellationToken cancellationToken = default);
        Task<Tournament?> GetByIdAsync(Guid tournamentId, CancellationToken cancellationToken = default);
    }
}

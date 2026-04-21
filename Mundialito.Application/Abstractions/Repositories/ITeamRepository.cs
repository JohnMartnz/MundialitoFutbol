using Mundialito.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Application.Abstractions.Repositories
{
    public interface ITeamRepository
    {
        Task AddAsync(Team team, CancellationToken cancellationToken = default);
        Task<Team?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
        Task<Team?> GetTeamByIdAsync(Guid teamId, CancellationToken cancellationToken = default);
    }
}

using Mundialito.Application.Common;
using Mundialito.Application.Features.Tournaments.Queries.GetTournaments;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Application.Abstractions.Repositories
{
    public interface ITournamentQueryRepository
    {
        Task<PagedResult<TournamentDto>> GetPagedAsync(int pageNumber, int pageSize, string? search, string? sortBy, string? sortDirection, CancellationToken cancellationToken = default);
    }
}

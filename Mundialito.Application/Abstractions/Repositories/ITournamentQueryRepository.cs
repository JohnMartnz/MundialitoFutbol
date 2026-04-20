using Mundialito.Application.Common;
using Mundialito.Application.Features.Tournaments.Queries.GetTournaments;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Application.Abstractions.Repositories
{
    public interface ITournamentQueryRepository
    {
        Task<PagedResult<TournamentDto>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    }
}

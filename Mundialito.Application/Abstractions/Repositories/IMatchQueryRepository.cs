using Mundialito.Application.Common;
using Mundialito.Application.Features.Matches.Queries.GetMatches;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Application.Abstractions.Repositories
{
    public interface IMatchQueryRepository
    {
        Task<PagedResult<MatchesResponse>> GetMatchesAsync(QueryParams queryParams, Guid? tournamentId, Guid? teamId, CancellationToken cancellationToken);
    }
}

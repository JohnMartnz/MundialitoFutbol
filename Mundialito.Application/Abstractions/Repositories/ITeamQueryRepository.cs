using Mundialito.Application.Common;
using Mundialito.Application.Features.Teams.Queries.GetTeams;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Application.Abstractions.Repositories
{
    public interface ITeamQueryRepository
    {
        Task <PagedResult<TeamsResponse>> GetPagedAsync(QueryParams queryParams, CancellationToken cancellationToken = default);
    }
}

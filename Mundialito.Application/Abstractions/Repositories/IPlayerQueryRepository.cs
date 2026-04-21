using Mundialito.Application.Common;
using Mundialito.Application.Features.Players.Queries.GetPlayers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Application.Abstractions.Repositories
{
    public interface IPlayerQueryRepository
    {
        Task<PagedResult<PlayersResponse>> GetPlayersAsync(QueryParams queryParams, Guid? teamId, CancellationToken cancellationToken);
    }
}

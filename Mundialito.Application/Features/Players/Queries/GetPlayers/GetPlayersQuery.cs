using MediatR;
using Mundialito.Application.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Application.Features.Players.Queries.GetPlayers
{
    public record GetPlayersQuery(QueryParams QueryParams, Guid? TeamId) : IRequest<PagedResult<PlayersResponse>>;
}

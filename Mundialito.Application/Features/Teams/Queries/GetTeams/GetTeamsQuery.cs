using MediatR;
using Mundialito.Application.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Application.Features.Teams.Queries.GetTeams
{
    public record GetTeamsQuery(
        QueryParams Params
    ) : IRequest<PagedResult<TeamsResponse>>;
}

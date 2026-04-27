using MediatR;
using Mundialito.Application.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Application.Features.Matches.Queries.GetMatches
{
    public record GetMatchesQuery(QueryParams QueryParams, Guid? TournamentId, Guid? TeamId) : IRequest<PagedResult<MatchesResponse>>;
}

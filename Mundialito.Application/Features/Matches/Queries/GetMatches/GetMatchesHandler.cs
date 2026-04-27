using MediatR;
using Mundialito.Application.Abstractions.Repositories;
using Mundialito.Application.Common;
using Mundialito.Application.Features.Players.Queries.GetPlayers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Application.Features.Matches.Queries.GetMatches
{
    public class GetMatchesHandler : IRequestHandler<GetMatchesQuery, PagedResult<MatchesResponse>>
    {
        private readonly IMatchQueryRepository _matchQueryRepository;

        public GetMatchesHandler(IMatchQueryRepository matchQueryRepository)
        {
            _matchQueryRepository = matchQueryRepository;
        }

        public async Task<PagedResult<MatchesResponse>> Handle(GetMatchesQuery request, CancellationToken cancellationToken)
        {
            return await _matchQueryRepository.GetMatchesAsync(request.QueryParams, request.TournamentId, request.TeamId, cancellationToken);
        }
    }
}

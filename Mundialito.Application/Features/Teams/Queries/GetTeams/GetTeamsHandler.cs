using MediatR;
using Mundialito.Application.Abstractions.Data;
using Mundialito.Application.Abstractions.Repositories;
using Mundialito.Application.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Application.Features.Teams.Queries.GetTeams
{
    public class GetTeamsHandler : IRequestHandler<GetTeamsQuery, PagedResult<TeamsResponse>>
    {
        private readonly ITeamQueryRepository _teamQueryRepository;

        public GetTeamsHandler(ITeamQueryRepository teamQueryRepository)
        {
            _teamQueryRepository = teamQueryRepository;
        }

        public async Task<PagedResult<TeamsResponse>> Handle(GetTeamsQuery request, CancellationToken cancellationToken)
        {
            return await _teamQueryRepository.GetPagedAsync(request.Params, cancellationToken);
        }
    }
}

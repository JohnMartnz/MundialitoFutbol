using MediatR;
using Mundialito.Application.Abstractions.Repositories;
using Mundialito.Application.Common;
using Mundialito.Domain.Common;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace Mundialito.Application.Features.Players.Queries.GetPlayers
{
    public class GetPlayersHandler : IRequestHandler<GetPlayersQuery, PagedResult<PlayersResponse>>
    {
        private readonly IPlayerQueryRepository _playerQueryRepository;

        public GetPlayersHandler(IPlayerQueryRepository playerQueryRepository)
        {
            _playerQueryRepository = playerQueryRepository;
        }

        public async Task<PagedResult<PlayersResponse>> Handle(GetPlayersQuery request, CancellationToken cancellationToken)
        {
            return await _playerQueryRepository.GetPlayersAsync(request.QueryParams, request.TeamId, cancellationToken);
        }
    }
}

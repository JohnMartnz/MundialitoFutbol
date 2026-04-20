using MediatR;
using Mundialito.Application.Abstractions.Repositories;
using Mundialito.Application.Common;
using Mundialito.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Application.Features.Tournaments.Queries.GetTournaments
{
    public class GetTournamentsQueryHandler : IRequestHandler<GetTournamentsQuery, PagedResult<TournamentDto>>
    {
        private readonly ITournamentQueryRepository _repository;

        public GetTournamentsQueryHandler(ITournamentQueryRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<TournamentDto>> Handle(GetTournamentsQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetPagedAsync(request.PageNumber, request.PageSize, request.Search, request.SortBy, request.SortDirection, cancellationToken);
        }
    }
}

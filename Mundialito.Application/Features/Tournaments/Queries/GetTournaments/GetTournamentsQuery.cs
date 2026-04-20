using MediatR;
using Mundialito.Application.Common;
using Mundialito.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Application.Features.Tournaments.Queries.GetTournaments
{
    public record GetTournamentsQuery(
        int PageNumber = 1,
        int PageSize = 10,
        string? Search = null,
        string? SortBy = null,
        string? SortDirection = null
    ) : IRequest<PagedResult<TournamentDto>>;
}

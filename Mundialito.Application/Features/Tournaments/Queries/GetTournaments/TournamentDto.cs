using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Application.Features.Tournaments.Queries.GetTournaments
{
    public record TournamentDto(Guid Id, string Name, DateTime StartDate, DateTime EndDate);
}

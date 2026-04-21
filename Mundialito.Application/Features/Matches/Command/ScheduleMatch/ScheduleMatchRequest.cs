using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Application.Features.Matches.Command.ScheduleMatch
{
    public record ScheduleMatchRequest(
        Guid TournamentId,
        Guid HomeTeamId,
        Guid VisitingTeamId,
        DateTime MatchDate
    );
}

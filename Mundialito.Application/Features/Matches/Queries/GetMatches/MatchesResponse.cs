using Mundialito.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Application.Features.Matches.Queries.GetMatches
{
    public class MatchesResponse
    {
        public Guid Id { get; init; }
        public Guid TournamentId { get; init; }
        public Guid HomeTeamId { get; init; }
        public Guid VisitingTeamId { get; init; }
        public int HomeTeamGoals { get; init; }
        public int VisitingTeamGoals { get; init; }
        public DateTime MatchDate { get; init; }
        public MatchStatus Status { get; init; }
        public string HomeTeamName { get; init; } = string.Empty;
        public string VisitingTeamName { get; init; } = string.Empty;
    }
}

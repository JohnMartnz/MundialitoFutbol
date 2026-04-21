using MediatR;
using Mundialito.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Application.Features.Matches.Command.ScheduleMatch
{
    public record ScheduleMatchCommand : IRequest<Result<Guid>> {
        public Guid TournamentId { get; init; }
        public Guid HomeTeamId { get; init; }
        public Guid VisitingTeamId { get; init; }
        public DateTime MatchDate { get; init; }
    }
}

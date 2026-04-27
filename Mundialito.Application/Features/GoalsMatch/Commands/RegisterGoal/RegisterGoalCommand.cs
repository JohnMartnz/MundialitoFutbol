using MediatR;
using Mundialito.Application.Features.Matches.Command;
using Mundialito.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Application.Features.GoalsMatch.Commands.RegisterGoal
{
    public record RegisterGoalCommand(
        Guid MatchId,
        Guid PlayerId,
        Guid TeamScoredId,
        int? MinuteScored
    ) : IRequest<Result<MatchStatusResponse>>;
}

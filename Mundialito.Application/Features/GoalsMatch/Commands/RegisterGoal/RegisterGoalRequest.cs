using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Application.Features.GoalsMatch.Commands.RegisterGoal
{
    public record RegisterGoalRequest(
        Guid PlayerId,
        Guid TeamScoredId,
        int? MinuteScored
    );
}

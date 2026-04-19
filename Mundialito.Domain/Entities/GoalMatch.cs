using Mundialito.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Domain.Entities
{
    public class GoalMatch : BaseEntity
    {
        public Guid PlayerId { get; private set; }
        public Guid MatchId { get; private set; }
        public Guid TeamScoredId { get; private set; }
        public int MinuteScored { get; private set; }

        private GoalMatch() { }

        public GoalMatch(Guid playerId, Guid matchId, Guid teamScoredId, int minuteScored)
        {
            Id = Guid.NewGuid();
            PlayerId = playerId;
            MatchId = matchId;
            TeamScoredId = teamScoredId;
            MinuteScored = minuteScored;
        }
    }
}

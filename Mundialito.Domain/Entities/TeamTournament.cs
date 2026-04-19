using Mundialito.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Domain.Entities
{
    public class TeamTournament : BaseEntity
    {
        public Guid TournamentId { get; private set; }
        public Guid TeamId { get; private set; }

        private TeamTournament() { }

        public TeamTournament(Guid tournamentId, Guid teamId)
        {
            Id = Guid.NewGuid();
            TournamentId = tournamentId;
            TeamId = teamId;
        }
    }
}

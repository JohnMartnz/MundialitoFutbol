using Mundialito.Domain.Common;
using Mundialito.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Domain.Entities
{
    public class Match : BaseEntity
    {
        public Guid TournamentId { get; private set; }
        public Guid HomeTeamId { get; private set; }
        public Guid VisitingTeamId { get; private set; }
        public Team? HomeTeam { get; private set; }
        public Team? VisitingTeam { get; private set; }
        public int HomeTeamGoals { get; private set; }
        public int VisitingTeamGoals { get; private set; }
        public DateTime MatchDate { get; private set; }
        public MatchStatus Status { get; private set; }

        private Match() { }

        public Result StartMatch()
        {
            if (Status != MatchStatus.Scheleduled)
            {
                return Result.Failure("Solo se puede iniciar un partido programado.");
            }
            Status = MatchStatus.InProgress;
            return Result.Success();
        }

        public Result RegisterGoal(Guid teamScoredId)
        {
            if (Status != MatchStatus.InProgress)
            {
                return Result.Failure("Solo se pueden registrar goles en un partido en progreso.");
            }

            if(teamScoredId != HomeTeamId && teamScoredId != VisitingTeamId)
            {
                return Result.Failure("El equipo anotador no participa en este partido.");
            }

            if(teamScoredId == HomeTeamId)
            {
                HomeTeamGoals++;
            }

            if (teamScoredId == VisitingTeamId)
            {
                VisitingTeamGoals++;
            }
            return Result.Success();
        }

        public Result EndMatch()
        {
            if (Status != MatchStatus.InProgress)
            {
                return Result.Failure("Solo se puede finalizar un partido en progreso.");
            }
            Status = MatchStatus.Finished;
            return Result.Success();
        }

        public static Match Scheledule(Guid tournamentId, Guid homeTeamId, Guid visitingTeamId, DateTime matchDate)
        {
            return new Match()
            {
                Id = Guid.NewGuid(),
                TournamentId = tournamentId,
                HomeTeamId = homeTeamId,
                VisitingTeamId = visitingTeamId,
                HomeTeamGoals = 0,
                VisitingTeamGoals = 0,
                MatchDate = matchDate,
                Status = MatchStatus.Scheleduled
            };
        }

        public void UpdateMatchStatus()
        {
            if(Status == MatchStatus.Scheleduled)
            {
                Status = MatchStatus.InProgress;
                return;
            }
            
            if(Status == MatchStatus.InProgress)
            {
                Status = MatchStatus.Finished;
            }
        }
    }
}

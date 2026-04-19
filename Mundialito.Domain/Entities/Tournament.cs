using Mundialito.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Domain.Entities
{
    public class Tournament : BaseEntity
    {
        public string Name { get; private set; } = string.Empty;
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }

        private readonly List<TeamTournament> _teams = new List<TeamTournament>();
        public IReadOnlyCollection<TeamTournament> Teams => _teams.AsReadOnly();

        private Tournament() { }

        public Tournament(string name, DateTime startDate, DateTime endDate)
        {
            Id = Guid.NewGuid();
            Name = name;
            StartDate = startDate;
            EndDate = endDate;
        }

        public Result AddTeam(Guid teamId)
        {
            if(_teams.Exists(team => team.TeamId == teamId))
            {
                return Result.Failure("El equipo ya está registrado en el torneo.");
            }

            _teams.Add(new TeamTournament(Id, teamId));
            return Result.Success();
        }
    }
}

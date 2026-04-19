using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Application.Features.Tournaments.Commands.CreateTournament
{
    public record CreateTournamentRequest
    {
        public string Name { get; init; } = string.Empty;
        public DateTime StartDate { get; init; }
        public DateTime EndDate
        {
            get; init;
        }
    }
}

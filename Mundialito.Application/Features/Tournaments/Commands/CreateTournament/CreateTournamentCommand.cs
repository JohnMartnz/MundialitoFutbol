using MediatR;
using Mundialito.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Application.Features.Tournaments.Commands.CreateTournament
{
    public record CreateTournamentCommand(
        string Name,
        DateTime StartDate,
        DateTime EndDate
    ) : IRequest<Result<Guid>>;
}

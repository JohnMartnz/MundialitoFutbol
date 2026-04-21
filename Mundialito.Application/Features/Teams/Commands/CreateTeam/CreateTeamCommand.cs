using MediatR;
using Mundialito.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Application.Features.Teams.Commands.CreateTeam
{
    public record CreateTeamCommand(string Name) : IRequest<Result<Guid>>;
}

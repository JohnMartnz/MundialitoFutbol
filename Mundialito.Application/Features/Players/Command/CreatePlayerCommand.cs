using MediatR;
using Mundialito.Domain.Common;
using Mundialito.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Application.Features.Players.Command
{
    public record CreatePlayerCommand(string Name, PositionPlayer PositionPlayer, Guid TeamId) : IRequest<Result<Guid>>;
}

using Mundialito.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Application.Features.Players.Command
{
    public record CreatePlayerRequest(string Name, PositionPlayer PositionPlayer, Guid TeamId);
}

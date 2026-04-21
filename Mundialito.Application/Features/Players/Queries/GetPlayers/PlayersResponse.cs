using Mundialito.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Application.Features.Players.Queries.GetPlayers
{
    public record PlayersResponse(Guid Id, string Name, PositionPlayer Position, Guid TeamId, string TeamName);
}

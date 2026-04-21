using Mundialito.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Application.Features.Matches.Command
{
    public record MatchStatusResponse(Guid Id, MatchStatus Status);
}

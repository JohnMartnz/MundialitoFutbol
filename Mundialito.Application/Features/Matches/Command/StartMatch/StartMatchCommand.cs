using MediatR;
using Mundialito.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Application.Features.Matches.Command.StartMatch
{
    public record StartMatchCommand(Guid MatchId) : IRequest<Result<MatchStatusResponse>>;
}

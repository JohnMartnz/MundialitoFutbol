using Mundialito.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Domain.Events
{public record TournamentCreatedEvent(
        Guid Id, 
        Guid TournamentId, 
        string TournamentName, 
        DateTime OccurredAt
    ) : IDomainEvent;
    
}

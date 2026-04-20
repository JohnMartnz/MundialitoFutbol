using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Domain.Common
{
    public interface IDomainEvent
    {
        Guid Id { get; }
        DateTime OccurredAt { get; }
    }
}

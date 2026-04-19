using Mundialito.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Application.Abstractions
{
    public interface IIdempotencyService
    {
        Task<IdempotencyRequest?> GetAsync(Guid key,  CancellationToken cancellationToken = default);
        Task SaveAsync(IdempotencyRequest request, CancellationToken cancellationToken = default);
    }
}

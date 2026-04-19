using Microsoft.EntityFrameworkCore;
using Mundialito.Application.Abstractions;
using Mundialito.Domain.Entities;
using Mundialito.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Infrastructure.Idempotency
{
    public class IdempotencyService : IIdempotencyService
    {
        private readonly MundialitoDbContext _dbContext;

        public IdempotencyService(MundialitoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IdempotencyRequest?> GetAsync(Guid key, CancellationToken cancellationToken)
        {
            return await _dbContext.IdempotencyRequests.FirstOrDefaultAsync(request => request.Id == key, cancellationToken);
        }

        public async Task SaveAsync(IdempotencyRequest request, CancellationToken cancellationToken)
        {
            _dbContext.IdempotencyRequests.Add(request);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}

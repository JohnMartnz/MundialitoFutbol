using Mundialito.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MundialitoDbContext _context;

        public UnitOfWork(MundialitoDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}

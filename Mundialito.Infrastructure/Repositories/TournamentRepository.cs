using Microsoft.EntityFrameworkCore;
using Mundialito.Application.Abstractions.Repositories;
using Mundialito.Domain.Entities;
using Mundialito.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Infrastructure.Repositories
{
    public class TournamentRepository : ITournamentRepository
    {
        private readonly MundialitoDbContext _context;

        public TournamentRepository(MundialitoDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Tournament tournament, CancellationToken cancellationToken)
        {
            await _context.AddAsync(tournament, cancellationToken);
        }

        public async Task<bool> NameExistsAsync(string name, CancellationToken cancellationToken)
        {
            return await _context.Tournaments.AnyAsync(tournament =>  tournament.Name == name, cancellationToken);
        }
    }
}

using Mundialito.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Application.Abstractions.Repositories
{
    public interface IGoalMatchRepository
    {
        Task AddAsync(GoalMatch goal, CancellationToken cancellationToken = default);
    }
}

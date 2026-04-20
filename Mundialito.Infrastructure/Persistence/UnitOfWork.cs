using MediatR;
using Microsoft.Extensions.Logging;
using Mundialito.Application.Abstractions;
using Mundialito.Application.Common.Interfaces;
using Mundialito.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MundialitoDbContext _context;
        private readonly IPublisher _publisher;
        private readonly ILogger<UnitOfWork> _logger;

        public UnitOfWork(MundialitoDbContext context, IPublisher publisher, ILogger<UnitOfWork> logger)
        {
            _context = context;
            _publisher = publisher;
            _logger = logger;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var events = _context.ChangeTracker.Entries<BaseEntity>()
                .SelectMany(e => e.Entity.DomainEvents)
                .ToList();

            await _context.SaveChangesAsync(cancellationToken);

            foreach(var domainEvent in events)
            {
                _logger.LogInformation("Publishing domain event {EventType} with {EventId} at {OcurredAt}", domainEvent.GetType().Name, domainEvent.Id, domainEvent.OccurredAt);

                var notification = new DomainEventNotification<IDomainEvent>(domainEvent);
                await _publisher.Publish(notification, cancellationToken);
            }

            _context.ChangeTracker.Entries<BaseEntity>()
                .ToList()
                .ForEach(e => e.Entity.ClearDomainEvents());
        }
    }
}

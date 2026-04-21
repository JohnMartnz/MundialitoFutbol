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
            var entities = _context.ChangeTracker.Entries<BaseEntity>()
                .Select(e => e.Entity)
                .Where(e => e.DomainEvents.Any())
                .ToList();

            var events = entities.SelectMany(e => e.DomainEvents).ToList();

            await _context.SaveChangesAsync(cancellationToken);

            entities.ForEach(e => e.ClearDomainEvents());


            foreach(var domainEvent in events)
            {
                _logger.LogInformation("Publishing domain event {EventType} with {EventId} at {OcurredAt}", domainEvent.GetType().Name, domainEvent.Id, domainEvent.OccurredAt);

                var notification = new DomainEventNotification<IDomainEvent>(domainEvent);
                
                await _publisher.Publish(new DomainEventNotification<IDomainEvent>(domainEvent), cancellationToken);
            }
        }
    }
}

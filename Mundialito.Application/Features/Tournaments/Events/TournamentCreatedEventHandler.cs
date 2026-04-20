using MediatR;
using Microsoft.Extensions.Logging;
using Mundialito.Application.Abstractions;
using Mundialito.Domain.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Application.Features.Tournaments.Events
{
    public class TournamentCreatedEventHandler: INotificationHandler<DomainEventNotification<TournamentCreatedEvent>>
    {
        private readonly ILogger<TournamentCreatedEventHandler> _logger;

        public TournamentCreatedEventHandler(ILogger<TournamentCreatedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(DomainEventNotification<TournamentCreatedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;
            _logger.LogInformation("Domain event handled: Tournament created | Id: {TournamentId} | Name: {TournamentName} | At: {OcuuredAt}",
                domainEvent.TournamentId, 
                domainEvent.TournamentName, 
                domainEvent.OccurredAt);

            return Task.CompletedTask;
        }
    }
}

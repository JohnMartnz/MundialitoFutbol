using MediatR;
using Microsoft.Extensions.Logging;
using Mundialito.Application.Abstractions.Repositories;
using Mundialito.Application.Common.Interfaces;
using Mundialito.Domain.Common;
using Mundialito.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Application.Features.Tournaments.Commands.CreateTournament
{
    public class CreateTournamentHandler : IRequestHandler<CreateTournamentCommand, Result<Guid>>
    {
        private readonly ITournamentRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateTournamentHandler> _logger;

        public CreateTournamentHandler(ITournamentRepository repository, IUnitOfWork unitOfWork, ILogger<CreateTournamentHandler> logger)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<Guid>> Handle(CreateTournamentCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating tournament {TournamentName}", command.Name);
            bool nameExists = await _repository.NameExistsAsync(command.Name, cancellationToken);

            if (nameExists)
            {
                _logger.LogWarning("Tournament creation failed - name {TournamentName} already exists", command.Name);
                return Result<Guid>.Conflict("El nombre del torneo ya existe");
            }

            var tournament = new Tournament(command.Name, command.StartDate, command.EndDate);

            await _repository.AddAsync(tournament, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Tournament {TournamentName} created successfully with ID {TournamentId}", tournament.Name, tournament.Id);

            return Result<Guid>.Success(tournament.Id);
        }
    }
}

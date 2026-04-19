using MediatR;
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

        public CreateTournamentHandler(ITournamentRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(CreateTournamentCommand command, CancellationToken cancellationToken)
        {
            bool nameExists = await _repository.NameExistsAsync(command.Name, cancellationToken);

            if (nameExists)
            {
                return Result<Guid>.Failure("El nombre del torneo ya existe");
            }

            var tournament = new Tournament(command.Name, command.StartDate, command.EndDate);

            await _repository.AddAsync(tournament, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(tournament.Id);
        }
    }
}

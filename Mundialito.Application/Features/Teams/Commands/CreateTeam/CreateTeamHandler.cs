using MediatR;
using Mundialito.Application.Abstractions.Repositories;
using Mundialito.Application.Common.Interfaces;
using Mundialito.Domain.Common;
using Mundialito.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Application.Features.Teams.Commands.CreateTeam
{
    public class CreateTeamHandler : IRequestHandler<CreateTeamCommand, Result<Guid>>
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateTeamHandler(ITeamRepository teamRepository, IUnitOfWork unitOfWork)
        {
            _teamRepository = teamRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return Result<Guid>.Validation("El nombre del equipo es obligatorio.");
            }

            var existingTeam = await _teamRepository.GetByNameAsync(request.Name, cancellationToken);
            if (existingTeam != null)
            {
                return Result<Guid>.Conflict("Ya existe un equipo con ese nombre.");
            }

            
            var newTeam = new Team(request.Name);
            await _teamRepository.AddAsync(newTeam);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(newTeam.Id);
        }
    }
}

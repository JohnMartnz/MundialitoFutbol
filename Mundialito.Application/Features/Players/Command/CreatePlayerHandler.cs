using MediatR;
using Mundialito.Application.Abstractions.Repositories;
using Mundialito.Application.Common.Interfaces;
using Mundialito.Domain.Common;
using Mundialito.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Application.Features.Players.Command
{
    public class CreatePlayerHandler : IRequestHandler<CreatePlayerCommand, Result<Guid>>
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CreatePlayerHandler(IPlayerRepository playerRepository, ITeamRepository teamRepository, IUnitOfWork unitOfWork)
        {
            _playerRepository = playerRepository;
            _teamRepository = teamRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(CreatePlayerCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return Result<Guid>.Validation("El nombre del jugador no puede estar vacío.");
            }

            var existingTeam = await _teamRepository.GetTeamByIdAsync(request.TeamId, cancellationToken);
            if (existingTeam is null)
            {
                return Result<Guid>.NotFound($"El equipo con ID {request.TeamId} no existe.");
            }

            var player = new Player(request.Name, request.PositionPlayer, request.TeamId);
            await _playerRepository.AddAsync(player, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(player.Id);
        }
    }
}

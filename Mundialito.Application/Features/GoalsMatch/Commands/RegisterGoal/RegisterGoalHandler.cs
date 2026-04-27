using MediatR;
using Mundialito.Application.Abstractions.Repositories;
using Mundialito.Application.Common.Interfaces;
using Mundialito.Application.Features.Matches.Command;
using Mundialito.Domain.Common;
using Mundialito.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Application.Features.GoalsMatch.Commands.RegisterGoal
{
    public class RegisterGoalHandler : IRequestHandler<RegisterGoalCommand, Result<MatchStatusResponse>>
    {
        private readonly IMatchRepository _matchRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IGoalMatchRepository _goalMatchRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterGoalHandler(
            IMatchRepository matchRepository, 
            IPlayerRepository playerRepository, 
            IGoalMatchRepository goalMatchRepository,
            IUnitOfWork unitOfWork
        )
        {
            _matchRepository = matchRepository;
            _playerRepository = playerRepository;
            _goalMatchRepository = goalMatchRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<MatchStatusResponse>> Handle(RegisterGoalCommand request, CancellationToken cancellationToken)
        {
            var match = await _matchRepository.GetByIdAsync(request.MatchId, cancellationToken);
            if(match is null)
            {
                return Result<MatchStatusResponse>.NotFound($"El partido con el id {request.MatchId}, no existe.");
            }

            var player = await _playerRepository.GetByIdAsync(request.PlayerId, cancellationToken);
            if(player is null)
            {
                return Result<MatchStatusResponse>.NotFound($"El jugador con el id {request.PlayerId}, no existe.");
            }

            if(player.TeamId != request.TeamScoredId)
            {
                return Result<MatchStatusResponse>.BadRequest("El jugador no pertenece al equipo anotado.");
            }

            var result = match.RegisterGoal(request.TeamScoredId);
            if(!result.IsSuccess)
            {
                return Result<MatchStatusResponse>.BadRequest(result.ErrorMessage!);
            }

            var goal = new GoalMatch(request.PlayerId, request.MatchId, request.TeamScoredId, request.MinuteScored);

            await _goalMatchRepository.AddAsync(goal, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<MatchStatusResponse>.Success(new MatchStatusResponse(request.MatchId, match.Status));
        }
    }
}

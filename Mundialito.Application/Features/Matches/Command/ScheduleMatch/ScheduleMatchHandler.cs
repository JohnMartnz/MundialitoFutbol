using MediatR;
using Mundialito.Application.Abstractions.Repositories;
using Mundialito.Application.Common.Interfaces;
using Mundialito.Domain.Common;
using Mundialito.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Application.Features.Matches.Command.ScheduleMatch
{
    public class ScheduleMatchHandler : IRequestHandler<ScheduleMatchCommand, Result<Guid>>
    {
        private readonly IMatchRepository _matchRepository;
        private readonly ITournamentRepository _tournamentRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ScheduleMatchHandler(IMatchRepository matchRepository, ITournamentRepository tournamentRepository, ITeamRepository teamRepository, IUnitOfWork unitOfWork)
        {
            _matchRepository = matchRepository;
            _tournamentRepository = tournamentRepository;
            _teamRepository = teamRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(ScheduleMatchCommand  request, CancellationToken cancellationToken)
        {
            if(request.HomeTeamId == request.VisitingTeamId)
            {
                return Result<Guid>.BadRequest("El equipo local y visitante no pueden ser el mismo");
            }

            if(request.MatchDate <= DateTime.UtcNow)
            {
                return Result<Guid>.BadRequest("La fecha del partido no debe ser futura");
            }

            var tournament = await _tournamentRepository.GetByIdAsync(request.TournamentId, cancellationToken);
            if(tournament is null)
            {
                return Result<Guid>.NotFound($"El torneo con el id {request.TournamentId}, no existe.");
            }

            var homeTeam = await _teamRepository.GetByIdAsync(request.HomeTeamId, cancellationToken);
            if(homeTeam is null)
            {
                return Result<Guid>.NotFound($"El equipo local con el id {request.HomeTeamId}, no existe");
            }

            var visitingTeam = await _teamRepository.GetByIdAsync(request.VisitingTeamId, cancellationToken);
            if(visitingTeam is null)
            {
                return Result<Guid>.NotFound($"El equipo visitante con el id {request.VisitingTeamId}, no existe.");
            }

            var match = Match.Scheledule(request.TournamentId, request.HomeTeamId, request.VisitingTeamId, request.MatchDate);
            await _matchRepository.AddAsync(match, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(match.Id);
        }
    }
}

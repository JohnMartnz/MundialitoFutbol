using MediatR;
using Mundialito.Application.Abstractions.Repositories;
using Mundialito.Application.Common.Interfaces;
using Mundialito.Domain.Common;
using Mundialito.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Application.Features.Matches.Command.StartMatch
{
    public class StartMatchHandler : IRequestHandler<StartMatchCommand, Result<MatchStatusResponse>>
    {
        private readonly IMatchRepository _matchRepository;
        private readonly IUnitOfWork _unitOfWork;

        public StartMatchHandler(IMatchRepository matchRepository, IUnitOfWork unitOfWork)
        {
            _matchRepository = matchRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<MatchStatusResponse>> Handle(StartMatchCommand request, CancellationToken cancellationToken)
        {
            var match = await _matchRepository.GetByIdAsync(request.MatchId, cancellationToken);
            if(match is null)
            {
                return Result<MatchStatusResponse>.NotFound($"El partido con el Id {request.MatchId} no existe.");
            }

            var result = match.StartMatch();
            if(!result.IsSuccess)
            {
                return Result<MatchStatusResponse>.BadRequest(result.ErrorMessage!);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<MatchStatusResponse>.Success(new MatchStatusResponse(request.MatchId, match.Status));
        }
    }
}

using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Mundialito.Api.Extensions;
using Mundialito.Api.Filters;
using Mundialito.Application.Features.GoalsMatch.Commands.RegisterGoal;
using Mundialito.Application.Features.Matches.Command.ScheduleMatch;
using Mundialito.Application.Features.Matches.Command.StartMatch;
using Mundialito.Domain.Common;

namespace Mundialito.Api.Endpoints
{
    public static class MatchEndpoints
    {
        public static void MapMatchEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/matches").WithTags("Matches");
            group.MapPost("/", async (ScheduleMatchRequest request, ISender sender) =>
            {
                var command = new ScheduleMatchCommand
                {
                    TournamentId = request.TournamentId,
                    HomeTeamId = request.HomeTeamId,
                    VisitingTeamId = request.VisitingTeamId,
                    MatchDate = request.MatchDate
                };
                var result = await sender.Send(command);

                return result.ToHttpResult($"/api/matches/{result.Value}");
            })
            .AddEndpointFilter<IdempotencyFilter>()
            .WithName("ScheduleMatch")
            .WithSummary("Schedules a new match between two teams in a tournament.");

            group.MapPatch("/{matchId:guid}/start", async (Guid matchId, ISender sender) =>
            {
                var command = new StartMatchCommand(matchId);
                var result = await sender.Send(command);

                return result.ToHttpResult();
            })
            .WithName("StartMatch")
            .WithSummary("Starts a scheduled match, changing its status to 'In Progress'.");

            group.MapPatch("/{matchId:guid}/goal", async (Guid matchId, RegisterGoalRequest request, ISender sender) =>
            {
                var command = new RegisterGoalCommand(matchId, request.PlayerId, request.TeamScoredId, request.MinuteScored);
                var result = await sender.Send(command);

                return result.ToHttpResult();
            })
            .WithName("RegisterGoal")
            .WithSummary("Register a goal scored in a match, updating the match status and score accordingly.");
        }
    }
}

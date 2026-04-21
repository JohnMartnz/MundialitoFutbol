using MediatR;
using Microsoft.AspNetCore.Mvc;
using Mundialito.Api.Extensions;
using Mundialito.Api.Filters;
using Mundialito.Application.Common;
using Mundialito.Application.Features.Players.Command;
using Mundialito.Application.Features.Players.Queries.GetPlayers;

namespace Mundialito.Api.Endpoints
{
    public static class PlayerEnpoints
    {
        public static void MapPlayerEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/players").WithTags("Players");

            group.MapPost("/", async (CreatePlayerRequest request, ISender sender) =>
            {
                var command = new CreatePlayerCommand(request.Name, request.PositionPlayer, request.TeamId);
                var result = await sender.Send(command);
                return result.ToHttpResult($"/api/players/{result.Value}");
            })
            .AddEndpointFilter<IdempotencyFilter>()
            .WithName("CreatePlayer")
            .WithSummary("Create a new player");

            group.MapGet("/", async ([FromQuery] Guid ? teamId, [AsParameters] QueryParams queryParams, ISender sender) =>
            {
                var query = new GetPlayersQuery(queryParams, teamId);
                var result = await sender.Send(query);

                return Results.Ok(result);
            })
            .WithName("GetPlayers")
            .WithSummary("Get a list of players with pagination and optional filtering by team");
        }
    }
}

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Mundialito.Api.Extensions;
using Mundialito.Api.Filters;
using Mundialito.Application.Common;
using Mundialito.Application.Features.Teams.Commands.CreateTeam;
using Mundialito.Application.Features.Teams.Queries.GetTeams;

namespace Mundialito.Api.Endpoints
{
    public static class TeamEndpoints
    {
        public static void MapTeamEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/teams").WithTags("Teams");

            group.MapPost("/", async (CreateTeamRequest request, ISender sender) =>
            {
                var command = new CreateTeamCommand(request.Name);
                var result = await sender.Send(command);

                return result.ToHttpResult($"/api/teams/{result.Value}");
            })
            .AddEndpointFilter<IdempotencyFilter>()
            .WithName("CreateTeam")
            .WithSummary("Creates a new team");

            group.MapGet("/", async (ISender sender, [AsParameters] QueryParams queryParams) =>
            {
                if(queryParams.PageNumber <= 0 || queryParams.PageSize <= 0)
                {
                    return Results.BadRequest("Parámetros de paginación inválidos");
                }

                var query = new GetTeamsQuery(queryParams);
                var result = await sender.Send(query);

                return Results.Ok(result);
            })
            .WithName("GetTeams")
            .WithSummary("Retrieves a paginated list of teams with optional search and sorting");
        }
    }
}

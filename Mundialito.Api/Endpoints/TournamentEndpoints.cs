using MediatR;
using Mundialito.Api.Filters;
using Mundialito.Application.Features.Tournaments.Commands.CreateTournament;
using Mundialito.Application.Features.Tournaments.Queries.GetTournaments;

namespace Mundialito.Api.Endpoints
{
    public static class TournamentEndpoints
    {
        public static void MapTournamentEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/tournaments").WithTags("Tournaments");

            group.MapPost("/", async (CreateTournamentRequest request, ISender sender) =>
            {
                var command = new CreateTournamentCommand(request.Name, request.StartDate, request.EndDate);
                var result = await sender.Send(command);

                return result.IsSuccess
                    ? Results.Created($"/api/tournaments/{result.Value}", result.Value)
                    : Results.BadRequest(result.Error);
            })
            .AddEndpointFilter<IdempotencyFilter>()
            .WithName("CreateTournament")
            .WithSummary("Creates a new tournament");

            group.MapGet("/", async (ISender sender, int pageNumber = 1, int pageSize = 10, string? search = null, string? sortBy = null, string? sortDirection = null) =>
            {
                if(pageNumber <= 0 || pageSize <= 0)
                {
                    return Results.BadRequest("Parámetros de paginación inválidos");
                }

                var query = new GetTournamentsQuery(pageNumber, pageSize, search, sortBy, sortDirection);
                var result = await sender.Send(query);
                return Results.Ok(result);
            })
            .WithName("GetTournaments")
            .WithSummary("Retrieves a paginated list of tournaments");
        }
    }
}

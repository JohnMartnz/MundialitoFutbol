using MediatR;
using Mundialito.Application.Features.Tournaments.Commands.CreateTournament;

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
            .WithName("CreateTournament")
            .WithSummary("Creates a new tournament");
        }
    }
}

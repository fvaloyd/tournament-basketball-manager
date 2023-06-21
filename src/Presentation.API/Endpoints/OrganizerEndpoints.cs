using Application.Features.Organizers.Commands;
using Application.Features.Organizers.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.API.Endpoints;

public static class OrganizerEndpoints
{
    public static void MapOrganizerEndpoints(this IEndpointRouteBuilder builder)
    {
        var organizersGroup = builder
            .MapGroup("/api/organizers")
            .RequireCors("CorsPolicy")
            .CacheOutput()
            .RequireRateLimiting("GlobalLimiter");

        organizersGroup.MapPost("/", async ([FromBody] CreateOrganizerCommand command, ISender sender, CancellationToken ct) =>
        {
            var organizerCreatedId = await sender.Send(command, ct);
            return Results.Ok(organizerCreatedId);
        });

        organizersGroup.MapPost("/{id:guid}/tournaments", async (Guid id, string tournamentName, ISender sender, CancellationToken ct) =>
        {
            var tournamentCreatedId = await sender.Send(new CreateTournamentCommand { OrganizerId = id, TournamentName = tournamentName }, ct);
            return Results.Ok(tournamentCreatedId);
        });

        organizersGroup.MapPost("/{id:guid}/tournaments/teams/{teamId:guid}", async (Guid id, Guid teamId, ISender sender, CancellationToken ct) =>
        {
            await sender.Send(new RegisterTeamCommand { OrganizerId = id, TeamId = teamId }, ct);
            return Results.NoContent();
        });

        organizersGroup.MapDelete("/{id:guid}/tournaments/teams/{teamId:guid}", async (Guid id, Guid teamId, ISender sender, CancellationToken ct) =>
        {
            await sender.Send(new DiscardTeamCommand { TeamId = teamId, OrganizerId = id }, ct);
            return Results.NoContent();
        });

        organizersGroup.MapPost("/{id:guid}/tournaments/matches", async (Guid id, ISender sender, CancellationToken ct) =>
        {
            await sender.Send(new MatchTeamsCommand { OrganizerId = id }, ct);
            return Results.NoContent();
        });

        organizersGroup.MapDelete("/{id:guid}/tournaments", async (Guid id, ISender sender, CancellationToken ct) =>
        {
            await sender.Send(new FinishTournamentCommand { OrganizerId = id }, ct);
            return Results.NoContent();
        });

        organizersGroup.MapGet("/{id:guid}", async (Guid id, ISender sender, CancellationToken ct) =>
        {
            var organizer = await sender.Send(new GetOrganizerQuery { OrganizerId = id }, ct);
            return Results.Ok(organizer);
        });

        organizersGroup.MapGet("/{id:guid}/tournaments/matches", async (Guid id, ISender sender, CancellationToken ct) =>
        {
            var matches = await sender.Send(new GetTournamentMatchesQuery { OrganizerId = id }, ct);
            return Results.Ok(matches);
        });
    }
}

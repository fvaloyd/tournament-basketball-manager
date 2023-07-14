using Application.Features.Managers.Commands;
using Application.Features.Managers.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.API.Endpoints;

public static class ManagerEndpoints
{
    public static void MapManagerEndpoints(this IEndpointRouteBuilder builder)
    {
        var managersGroup = builder
            .MapGroup("api/managers")
            .RequireCors("CorsPolicy")
            .CacheOutput("OutputCachePolicy")
            .RequireRateLimiting("GlobalLimiter");

        managersGroup.MapGet("/", async (ISender sender, CancellationToken ct) =>
        {
            var managers = await sender.Send(new GetManagersQuery(), ct);
            return Results.Ok(managers);
        });
        managersGroup.MapGet("/{id:guid}", async (Guid id, ISender sender, CancellationToken ct) =>
        {
            var manager = await sender.Send(new GetManagerQuery { ManagerId = id }, ct);
            return Results.Ok(manager);
        });

        managersGroup.MapPost("/", async ([FromBody] CreateManagerCommand command, ISender sender, CancellationToken ct) =>
        {
            var managerCreatedId = await sender.Send(command, ct);
            return Results.Ok(managerCreatedId);
        });

        managersGroup.MapPost("/{id:guid}/teams", async (Guid id, string teamName, ISender sender, CancellationToken ct) =>
        {
            var teamIdCreated = await sender.Send(new CreateTeamCommand { ManagerId = id, TeamName = teamName }, ct);
            return Results.Ok(teamIdCreated);
        });

        managersGroup.MapDelete("/{id:guid}/teams", async (Guid id, ISender sender, CancellationToken ct) =>
        {
            await sender.Send(new DissolveTeamCommand { ManagerId = id }, ct);
            return Results.NoContent();
        });

        managersGroup.MapPost("/{id:guid}/teams/players/{playerId:guid}", async (Guid id, Guid playerId, ISender sender, CancellationToken ct) =>
        {
            await sender.Send(new DraftPlayerCommand { ManagerId = id, PlayerId = playerId }, ct);
            return Results.NoContent();
        });

        managersGroup.MapDelete("/{id:guid}/teams/players/{playerId:guid}", async (Guid id, Guid playerId, ISender sender, CancellationToken ct) =>
        {
            await sender.Send(new ReleasePlayerCommand { ManagerId = id, PlayerId = playerId }, ct);
            return Results.NoContent();
        });
    }
}
using Application.Features.Players.Commands;
using Application.Features.Players.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.API.Endpoints;

public static class PlayerEndpoints
{
    public static void MapPlayerEndpoints(this IEndpointRouteBuilder builder)
    {
        var playerGroup = builder
            .MapGroup("/api/players")
            .RequireCors("CorsPolicy")
            .CacheOutput()
            .RequireRateLimiting("GlobalLimiter");

        playerGroup.MapGet("/", async (ISender sender, CancellationToken ct) =>
        {
            var players = await sender.Send(new GetPlayersQuery(), ct);
            return Results.Ok(players);
        });

        playerGroup.MapPost("/", async ([FromBody] CreatePlayerCommand command, ISender sender, CancellationToken ct) =>
        {
            var playerCreatedId = await sender.Send(command, ct);
            return Results.Ok(playerCreatedId);
        });
    }
}

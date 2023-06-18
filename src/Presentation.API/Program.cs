using MediatR;
using Application;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Sql.Context;
using Microsoft.EntityFrameworkCore;
using Application.Features.Players.Queries;
using Application.Features.Players.Commands;
using Application.Features.Managers.Commands;
using Application.Features.Managers.Queries;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TournamentBasketballManagerDbContext>(
    opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("TournamentBasketballManagerDb"),
        s => s.MigrationsAssembly(typeof(Program).Assembly.FullName)), contextLifetime: ServiceLifetime.Transient, optionsLifetime: ServiceLifetime.Transient);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();

var app = builder.Build();

app.MapGet("/players", async (ISender sender, CancellationToken ct) =>
{
    var players = await sender.Send(new GetPlayersQuery(), ct);
    return Results.Ok(players);
});

app.MapPost("/players", async ([FromBody]CreatePlayerCommand command, ISender sender, CancellationToken ct) =>
{
    var playerCreatedId = await sender.Send(command, ct);
    return Results.Ok(playerCreatedId);
});

app.MapGet("/managers/{id:guid}", async (Guid id, ISender sender, CancellationToken ct) => {
    var manager = await sender.Send(new GetManagerQuery{ManagerId = id}, ct);
    return Results.Ok(manager);
});

app.MapPost("/managers", async ([FromBody]CreateManagerCommand command, ISender sender, CancellationToken ct) => {
    var managerCreatedId = await sender.Send(command, ct);
    return Results.Ok(managerCreatedId);
});

app.MapPost("/managers/{id:guid}/teams", async (Guid id, string teamName, ISender sender, CancellationToken ct) =>
{
    var teamIdCreated = await sender.Send(new CreateTeamCommand { ManagerId = id, TeamName = teamName}, ct);
    return Results.Ok(teamIdCreated);
});

app.MapDelete("/managers/{id:guid}/teams", async (Guid id, ISender sender, CancellationToken ct) =>
{
    await sender.Send(new DissolveTeamCommand { ManagerId = id}, ct);
    return Results.NoContent();
});

app.MapPost("/managers/{id:guid}/teams/players/{playerId:guid}", async (Guid id, Guid playerId, ISender sender, CancellationToken ct) =>
{
    await sender.Send(new DraftPlayerCommand { ManagerId = id, PlayerId = playerId}, ct);
    return Results.NoContent();
});

app.MapDelete("/managers/{id:guid}/teams/players/{playerId:guid}", async (Guid id, Guid playerId, ISender sender, CancellationToken ct) =>
{
    await sender.Send(new ReleasePlayerCommand{ ManagerId = id, PlayerId = playerId }, ct);
    return Results.NoContent();
});

app.Run();
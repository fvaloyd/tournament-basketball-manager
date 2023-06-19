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
using Application.Features.Organizers.Commands;
using Application.Features.Organizers.Queries;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TournamentBasketballManagerDbContext>(
    opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("TournamentBasketballManagerDb"),
        s => s.MigrationsAssembly(typeof(Program).Assembly.FullName)), contextLifetime: ServiceLifetime.Transient, optionsLifetime: ServiceLifetime.Transient);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();

var app = builder.Build();

var playersGroup = app.MapGroup("/api/players");
var managersGroup = app.MapGroup("api/managers");
var organizersGroup = app.MapGroup("/api/organizers");

playersGroup.MapGet("/", async (ISender sender, CancellationToken ct) =>
{
    var players = await sender.Send(new GetPlayersQuery(), ct);
    return Results.Ok(players);
});

playersGroup.MapPost("/", async ([FromBody] CreatePlayerCommand command, ISender sender, CancellationToken ct) =>
{
    var playerCreatedId = await sender.Send(command, ct);
    return Results.Ok(playerCreatedId);
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

managersGroup.MapPost("/id:guid}/teams", async (Guid id, string teamName, ISender sender, CancellationToken ct) =>
{
    var teamIdCreated = await sender.Send(new CreateTeamCommand { ManagerId = id, TeamName = teamName }, ct);
    return Results.Ok(teamIdCreated);
});

managersGroup.MapDelete("/id:guid}/teams", async (Guid id, ISender sender, CancellationToken ct) =>
{
    await sender.Send(new DissolveTeamCommand { ManagerId = id }, ct);
    return Results.NoContent();
});

managersGroup.MapPost("/id:guid}/teams/players/{playerId:guid}", async (Guid id, Guid playerId, ISender sender, CancellationToken ct) =>
{
    await sender.Send(new DraftPlayerCommand { ManagerId = id, PlayerId = playerId }, ct);
    return Results.NoContent();
});

managersGroup.MapDelete("/id:guid}/teams/players/{playerId:guid}", async (Guid id, Guid playerId, ISender sender, CancellationToken ct) =>
{
    await sender.Send(new ReleasePlayerCommand { ManagerId = id, PlayerId = playerId }, ct);
    return Results.NoContent();
});

organizersGroup.MapPost("/", async ([FromBody] CreateOrganizerCommand command, ISender sender, CancellationToken ct) =>
{
    var organizerCreatedId = await sender.Send(command, ct);
    return Results.Ok(organizerCreatedId);
});

organizersGroup.MapPost("/id:guid}/tournaments", async (Guid id, string tournamentName, ISender sender, CancellationToken ct) =>
{
    var tournamentCreatedId = await sender.Send(new CreateTournamentCommand { OrganizerId = id, TournamentName = tournamentName }, ct);
    return Results.Ok(tournamentCreatedId);
});

organizersGroup.MapPost("/{id:guid}/tournaments/teams/{teamId:guid}", async (Guid id, Guid teamId, ISender sender, CancellationToken ct) =>
{
    await sender.Send(new RegisterTeamCommand { OrganizerId = id, TeamId = teamId }, ct);
    return Results.NoContent();
});

organizersGroup.MapDelete("/id:guid}/tournaments/teams/{teamId:guid}", async (Guid id, Guid teamId, ISender sender, CancellationToken ct) =>
{
    await sender.Send(new DiscardTeamCommand { TeamId = teamId, OrganizerId = id }, ct);
    return Results.NoContent();
});

organizersGroup.MapPost("/id:guid}/tournaments/matches", async (Guid id, ISender sender, CancellationToken ct) =>
{
    await sender.Send(new MatchTeamsCommand { OrganizerId = id }, ct);
    return Results.NoContent();
});

organizersGroup.MapDelete("/id:guid}/tournaments", async (Guid id, ISender sender, CancellationToken ct) =>
{
    await sender.Send(new FinishTournamentCommand { OrganizerId = id }, ct);
    return Results.NoContent();
});

organizersGroup.MapGet("/id:guid}", async (Guid id, ISender sender, CancellationToken ct) =>
{
    var organizer = await sender.Send(new GetOrganizerQuery { OrganizerId = id }, ct);
    return Results.Ok(organizer);
});

organizersGroup.MapGet("/id:guid}/tournaments/matches", async (Guid id, ISender sender, CancellationToken ct) =>
{
    var matches = await sender.Send(new GetTournamentMatchesQuery { OrganizerId = id }, ct);
    return Results.Ok(matches);
});

app.Run();
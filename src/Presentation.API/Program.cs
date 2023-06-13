using MediatR;
using Application;
using Infrastructure;
using Infrastructure.Sql.Context;
using Application.Features.Players.Queries;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSqlServer<TournamentBasketballManagerDbContext>(
    builder.Configuration.GetConnectionString("TournamentBasketballManagerDb"),
    s => s.MigrationsAssembly(typeof(Program).Assembly.FullName));

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();

var app = builder.Build();

app.MapGet("/players", async (ISender sender, CancellationToken ct) =>
{
    var players = await sender.Send(new GetPlayersQuery(), ct);
    return Results.Ok(players);
});

app.Run();
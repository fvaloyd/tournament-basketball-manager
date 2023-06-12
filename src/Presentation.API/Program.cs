using Application;
using Application.Features.Players.Queries;
using Infrastructure;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

app.MapGet("/players", async (ISender sender, CancellationToken ct) =>
{
    var players = await sender.Send(new GetPlayersQuery(), ct);
    return Results.Ok(players);
});

app.Run();
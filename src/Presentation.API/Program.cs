using Application;
using Infrastructure;
using Presentation.API.Endpoints;
using Presentation.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureSqlServerDbContext(builder.Configuration);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services
    .ConfigureCors()
    .ConfigureRateLimiting();

var app = builder.Build();

app.UseCors("CorsPolicy");
app.ConfigureExceptionHandlerMiddlware();

app.MapPlayerEndpoints();
app.MapManagerEndpoints();
app.MapOrganizerEndpoints();

app.MigrateDatabase();

app.Run();
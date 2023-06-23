using Application;
using Infrastructure;
using Presentation.API.Endpoints;
using Presentation.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services
    .ConfigureSqlServerDbContext(builder.Configuration)
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration);

builder.Services
    .ConfigureCors()
    .ConfigureRateLimiting()
    .ConfigureOutputCache()
    .ConfigureSerilog();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.ConfigureExceptionHandlerMiddlware();

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("CorsPolicy");
app.MapFallbackToFile("index.html");

app.MapRazorPages();

app.MapPlayerEndpoints();
app.MapManagerEndpoints();
app.MapOrganizerEndpoints();

app.MigrateDatabase();

app.Run();
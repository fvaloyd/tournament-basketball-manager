using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Presentation.Web;
using Presentation.Web.Pages.Organizers;
using Presentation.Web.Pages.Players;
using Shared;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddSingleton<PlayersState>();
builder.Services.AddSingleton<OrganizerState>();

builder.Services.AddMudServices();
builder.Services.AddApiRefitClients(builder.HostEnvironment.BaseAddress);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using W40k_CheatSheet.Client;
using W40k_CheatSheet.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var supabaseUrl = builder.Configuration["Supabase:Url"] ?? "";
var supabaseKey = builder.Configuration["Supabase:AnonKey"] ?? "";

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped(sp => new DetachmentEffectsService(
    sp.GetRequiredService<HttpClient>(),
    sp.GetRequiredService<IJSRuntime>()));
builder.Services.AddScoped(sp =>
    new CloudRosterService(
        sp.GetRequiredService<HttpClient>(),
        sp.GetRequiredService<IJSRuntime>(),
        supabaseUrl, supabaseKey));

await builder.Build().RunAsync();

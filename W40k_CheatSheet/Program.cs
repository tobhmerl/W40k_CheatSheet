using Microsoft.EntityFrameworkCore;
using W40k_CheatSheet.Components;
using W40k_CheatSheet.Data;
using W40k_CheatSheet.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

// ── Database + Identity ──
builder.Services.AddDbContext<RosterDbContext>(o =>
    o.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")
                ?? "Data Source=rosters.db"));

builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<AppUser>()
    .AddEntityFrameworkStores<RosterDbContext>();

// ── CORS for GitHub Pages client ──
builder.Services.AddCors(options =>
{
    options.AddPolicy("ClientApp", policy =>
    {
        policy.WithOrigins(
                "https://tobhmerl.github.io",
                "https://localhost:7298",
                "http://localhost:5298")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

// ── Auto-create/migrate database on startup ──
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<RosterDbContext>();
    await db.Database.EnsureCreatedAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseWhen(
    context => !context.Request.Path.StartsWithSegments("/_framework") &&
               !context.Request.Path.StartsWithSegments("/_content") &&
               !context.Request.Path.StartsWithSegments("/api"),
    branch => branch.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true)
);
app.UseHttpsRedirection();
app.UseCors("ClientApp");

app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

// ── API endpoints ──
app.MapGroup("/api/auth").MapIdentityApi<AppUser>();
app.MapRosterApi();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(W40k_CheatSheet.Client._Imports).Assembly);

app.Run();

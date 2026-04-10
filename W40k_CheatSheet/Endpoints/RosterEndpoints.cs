using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using W40k_CheatSheet.Data;

namespace W40k_CheatSheet.Endpoints;

public static class RosterEndpoints
{
    public record RosterListItem(Guid Id, string Name, string Faction, string Detachment, int Points, DateTime LastModified);
    public record RosterDetail(Guid Id, string Name, string Faction, string Detachment, int Points, string DataJson, DateTime LastModified);
    public record SaveRosterRequest(string Name, string Faction, string Detachment, int Points, string DataJson);

    public static void MapRosterApi(this WebApplication app)
    {
        var group = app.MapGroup("/api/rosters").RequireAuthorization();

        group.MapGet("/", async (RosterDbContext db, ClaimsPrincipal user) =>
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier)!;
            return await db.SavedRosters
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.LastModified)
                .Select(r => new RosterListItem(r.Id, r.Name, r.Faction, r.Detachment, r.Points, r.LastModified))
                .ToListAsync();
        });

        group.MapGet("/{id:guid}", async (Guid id, RosterDbContext db, ClaimsPrincipal user) =>
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var roster = await db.SavedRosters
                .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);
            if (roster is null) return Results.NotFound();
            return Results.Ok(new RosterDetail(roster.Id, roster.Name, roster.Faction, roster.Detachment, roster.Points, roster.DataJson, roster.LastModified));
        });

        group.MapPost("/", async (SaveRosterRequest req, RosterDbContext db, ClaimsPrincipal user) =>
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var existing = await db.SavedRosters
                .FirstOrDefaultAsync(r => r.UserId == userId && r.Name == req.Name);

            if (existing is not null)
            {
                existing.Faction = req.Faction;
                existing.Detachment = req.Detachment;
                existing.Points = req.Points;
                existing.DataJson = req.DataJson;
                existing.LastModified = DateTime.UtcNow;
            }
            else
            {
                existing = new SavedRoster
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Name = req.Name,
                    Faction = req.Faction,
                    Detachment = req.Detachment,
                    Points = req.Points,
                    DataJson = req.DataJson,
                    LastModified = DateTime.UtcNow
                };
                db.SavedRosters.Add(existing);
            }

            await db.SaveChangesAsync();
            return Results.Ok(new RosterListItem(existing.Id, existing.Name, existing.Faction, existing.Detachment, existing.Points, existing.LastModified));
        });

        group.MapDelete("/{id:guid}", async (Guid id, RosterDbContext db, ClaimsPrincipal user) =>
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var roster = await db.SavedRosters
                .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);
            if (roster is null) return Results.NotFound();
            db.SavedRosters.Remove(roster);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });
    }
}

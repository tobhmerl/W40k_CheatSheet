using Microsoft.AspNetCore.Identity;

namespace W40k_CheatSheet.Data;

public class AppUser : IdentityUser
{
    public List<SavedRoster> Rosters { get; set; } = [];
}

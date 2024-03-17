
using ExpressedRealms.DB.UserProfile.PlayerDBModels;

namespace ExpressedRealms.DB.Characters;

public class Character
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Background { get; set; }
    public Guid PlayerId { get; set; }

    public virtual Player Player { get; set; } = null!;
}
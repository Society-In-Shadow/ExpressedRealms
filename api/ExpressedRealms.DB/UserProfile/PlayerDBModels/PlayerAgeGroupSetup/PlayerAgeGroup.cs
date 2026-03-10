using ExpressedRealms.DB.UserProfile.PlayerDBModels.PlayerSetup;

namespace ExpressedRealms.DB.UserProfile.PlayerDBModels.PlayerAgeGroupSetup;

public class PlayerAgeGroup
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public virtual ICollection<Player> EventQuestions { get; set; } =
        new HashSet<Player>();
}

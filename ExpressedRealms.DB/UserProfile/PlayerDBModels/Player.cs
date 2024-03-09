namespace ExpressedRealms.DB.UserProfile.PlayerDBModels;

public class Player
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = null!;
    public short PlayerNumber { get; set; }
    public string Name { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string City { get; set; } = null!;
    public string State { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}

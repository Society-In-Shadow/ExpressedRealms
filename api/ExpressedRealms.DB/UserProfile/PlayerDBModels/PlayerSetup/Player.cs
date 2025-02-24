using Audit.EntityFramework;
using ExpressedRealms.DB.Characters;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.UserSetup;

namespace ExpressedRealms.DB.UserProfile.PlayerDBModels.PlayerSetup;

/*[AuditInclude]*/
public class Player
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = null!;
    public short PlayerNumber { get; set; }
    
    public string Name { get; set; } = null!;
    /*public DateTime LastLoginDate { get; set; }*/

    public virtual User User { get; set; } = null!;
    public virtual List<Character> Characters { get; set; } = new();
}

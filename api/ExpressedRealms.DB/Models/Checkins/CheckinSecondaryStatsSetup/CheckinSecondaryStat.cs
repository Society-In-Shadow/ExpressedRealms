using ExpressedRealms.DB.Models.Checkins.CheckinSetup;

namespace ExpressedRealms.DB.Models.Checkins.CheckinSecondaryStatsSetup;

public class CheckinSecondaryStat
{
    public int Id { get; set; }
    
    public int CheckinId { get; set; }
    public virtual Checkin Checkin { get; set; } = null!;
    
    public int Vitality { get; set; }
    public int Health { get; set; }
    public int Blood { get; set; }
    public int Rwp { get; set; }
    public int Psyche { get; set; }
    public int Mortis { get; set; }
    
    public int Mana { get; set; }
    public int Chi { get; set; }
    public int Essence { get; set; }
    public int Noumenon { get; set; }
}

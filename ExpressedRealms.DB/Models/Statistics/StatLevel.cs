namespace ExpressedRealms.DB.Models.Statistics;

public class StatLevel
{
    public byte Id { get; set; }
    public int Bonus { get; set; }
    public int XPCost { get; set; }
    
    public virtual List<StatDescriptionMapping> StatDescriptionMappings { get; set; } = null!;
}
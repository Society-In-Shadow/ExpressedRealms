namespace ExpressedRealms.DB.Models.Powers;

public class PowerAreaOfEffectType
{
    public byte Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    public virtual List<Power> Powers { get; set; } = null!;
}

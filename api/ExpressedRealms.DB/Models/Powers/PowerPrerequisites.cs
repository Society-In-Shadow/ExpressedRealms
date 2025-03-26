namespace ExpressedRealms.DB.Models.Powers;

public class PowerPrerequisites
{
    public int ParentPowerId { get; set; }
    public virtual Power ParentPower { get; set; } = null!;

    public int ChildPowerId { get; set; }
    public virtual Power ChildPower { get; set; } = null!;
}

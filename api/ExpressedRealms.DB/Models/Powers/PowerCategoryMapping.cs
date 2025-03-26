namespace ExpressedRealms.DB.Models.Powers;

public class PowerCategoryMapping
{
    public int PowerId { get; set; }
    public int CategoryId { get; set; }

    public virtual Power Power { get; set; } = null!;
    public virtual PowerCategory Category { get; set; } = null!;
}

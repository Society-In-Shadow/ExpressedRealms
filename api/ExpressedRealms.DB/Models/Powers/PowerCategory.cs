namespace ExpressedRealms.DB.Models.Powers;

public class PowerCategory
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    public virtual List<PowerCategoryMapping> PowerMappings { get; set; } = null!;
}

namespace ExpressedRealms.DB.Models.Statistics;

public class StatType
{
    public byte Id { get; set; }
    public string Name { get; set; }
    public string ShortName { get; set; }
    public string Description { get; set; }

    public virtual List<StatDescriptionMapping> StatDescriptionMappings { get; set; } = null!;
}

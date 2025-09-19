namespace ExpressedRealms.Characters.UseCases.ExperienceBreakdown;

public class ExperienceTotalMax
{
    public string Name { get; }
    public int Total { get; set; }
    public int Max { get; }
    public int TypeId { get; }

    public ExperienceTotalMax(
        string name,
        int total,
        int max,
        int typeId = 0
    )
    {
        Name = name;
        Total = total;
        Max = max;
        TypeId = typeId;
    }
}

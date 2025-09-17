namespace ExpressedRealms.Characters.UseCases.ExperienceBreakdown;

public class ExperienceTotalMax
{
    public string Name { get; }
    public int Total { get; set; }
    public int Max { get; }
    public bool IncludeInTotal { get; set; }
    public bool IncludeInMax { get; set; }
    public int TypeId { get; }

    public ExperienceTotalMax(
        string name,
        int total,
        int max,
        bool includeInTotal = true,
        bool includeInMax = true,
        int typeId = 0)
    {
        Name = name;
        Total = total;
        Max = max;
        IncludeInTotal = includeInTotal;
        IncludeInMax = includeInMax; 
        TypeId = typeId;
    }
}

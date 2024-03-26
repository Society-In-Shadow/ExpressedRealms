namespace ExpressedRealms.DB.Models.Expressions;

public class Expression
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string ShortDescription { get; set; } = null!;
    public string NavMenuImage { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Culture { get; set; } = null!;
    public string Alliances { get; set; } = null!;
    public string StrainedRelationships { get; set; } = null!;
    public string Advantages { get; set; } = null!;
    public string Disadvantages { get; set; } = null!;
    public string? MaterialWeakness { get; set; }
}

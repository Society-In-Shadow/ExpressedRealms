using ExpressedRealms.DB.Characters;

namespace ExpressedRealms.DB.Models.Expressions;

public class Expression
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string ShortDescription { get; set; } = null!;
    public string NavMenuImage { get; set; } = null!;

    public virtual List<ExpressionSection> ExpressionSections { get; set; } = null!;
    public virtual List<Character> Characters { get; set; } = null!;
}

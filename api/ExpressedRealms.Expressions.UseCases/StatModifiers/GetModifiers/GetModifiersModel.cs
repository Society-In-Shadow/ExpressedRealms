using ExpressedRealms.Expressions.UseCases.StatModifiers.Add;

namespace ExpressedRealms.Expressions.UseCases.StatModifiers.GetModifiers;

public class GetModifiersModel
{
    public int GroupId { get; set; }
    public SourceTableEnum Source { get; set; }
}

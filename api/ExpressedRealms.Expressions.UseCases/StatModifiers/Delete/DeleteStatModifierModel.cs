using ExpressedRealms.Expressions.UseCases.StatModifiers.Add;

namespace ExpressedRealms.Expressions.UseCases.StatModifiers.Delete;

public class DeleteStatModifierModel
{
    public int Id { get; set; }
    public int StatModifierGroupId { get; set; }
    public SourceTableEnum Source { get; set; }
}

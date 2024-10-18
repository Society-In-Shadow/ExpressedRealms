namespace ExpressedRealms.Repositories.Expressions.ExpressionTextSections.DTOs;

public class ExpressionTextSectionOptions
{
    public List<PotentialParentsDto> AvailableParents { get; set; } = null!;
    public List<SectionTypeDto> ExpressionSectionTypes { get; set; } = null!;
}

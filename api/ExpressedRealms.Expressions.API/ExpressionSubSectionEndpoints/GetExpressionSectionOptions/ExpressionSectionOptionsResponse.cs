namespace ExpressedRealms.Expressions.API.ExpressionSubSectionEndpoints.GetExpressionSectionOptions;

public class ExpressionSectionOptionsResponse
{
    public List<AvailableParentsDto> AvailableParents { get; set; } = new();
    public List<SectionTypeDto> SectionTypes { get; set; } = new();
}

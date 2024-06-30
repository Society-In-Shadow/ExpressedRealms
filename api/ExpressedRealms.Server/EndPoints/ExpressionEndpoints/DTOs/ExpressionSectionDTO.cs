namespace ExpressedRealms.Server.EndPoints.ExpressionEndpoints.DTOs;

public class ExpressionSectionDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Content { get; set; }
    public List<ExpressionSectionDTO> SubSections { get; set; } = new();
}

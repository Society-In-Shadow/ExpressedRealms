using ExpressedRealms.DB.Models.Knowledges.KnowledgeEducationLevels;

namespace ExpressedRealms.Expressions.Repository.CharacterFactions.Dtos;

public class CharacterFactionDto
{
    public int FactionLevelId { get; set; }
    public string? Approver { get; set; }
    public string? ApprovalReason { get; set; }
    public bool RequestedPromotion { get; set; }
    public string? RequestedPromotionReason { get; set; }
    public DateTimeOffset ApprovalDate { get; set; }
    public int? KnowledgeId { get; set; }
    public KnowledgeEducationLevel? KnowledgeLevel { get; set; }
    public string? KnowledgeSpecialization { get; set; }
    public string? CharacterNotes { get; set; }
}
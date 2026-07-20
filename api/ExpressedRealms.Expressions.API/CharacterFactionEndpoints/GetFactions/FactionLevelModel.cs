namespace ExpressedRealms.Expressions.API.CharacterFactionEndpoints.GetFactions;

public class FactionLevelModel
{
    public int FactionLevelId { get; set; }
    public string? Approver { get; set; }
    public string? ApprovalReason { get; set; }
    public bool RequestedPromotion { get; set; }
    public string? RequestedPromotionReason { get; set; }
    public DateTimeOffset ApprovalDate { get; set; }
    public bool HasKnowledge { get; set; }
    public bool HasKnowledgeLevel { get; set; }
    public bool? HasSpecialization { get; set; }
    public string? CharacterNotes { get; set; }
}

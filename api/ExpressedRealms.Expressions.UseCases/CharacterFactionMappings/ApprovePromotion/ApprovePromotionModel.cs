namespace ExpressedRealms.Expressions.UseCases.CharacterFactionMappings.ApprovePromotion;

public class ApprovePromotionModel
{
    public int CharacterId { get; set; }
    public int FactionLevelId { get; set; }
    public string? ApprovalReason { get; set; }
}

namespace ExpressedRealms.Expressions.UseCases.CharacterFactionMappings.RequestPromotion;

public class RequestPromotionModel
{
    public int CharacterId { get; set; }
    public int FactionLevelId { get; set; }
    public string? RequestReason { get; set; }
}

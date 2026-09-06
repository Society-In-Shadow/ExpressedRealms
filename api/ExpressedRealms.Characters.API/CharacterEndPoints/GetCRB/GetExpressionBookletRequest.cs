namespace ExpressedRealms.Characters.API.CharacterEndPoints.GetCRB;

public record GetExpressionBookletRequest()
{
    public bool? UseLatestApproved { get; init; }
};
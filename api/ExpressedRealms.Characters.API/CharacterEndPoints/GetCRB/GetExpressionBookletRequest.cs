namespace ExpressedRealms.Characters.API.CharacterEndPoints.GetCRB;

public record GetExpressionBookletRequest()
{
    public bool? UseLatestApproved { get; init; }
    public bool? OverwriteArchiveDiff { get; init; }
};

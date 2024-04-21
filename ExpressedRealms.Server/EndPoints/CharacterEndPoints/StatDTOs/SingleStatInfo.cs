namespace ExpressedRealms.Server.EndPoints.CharacterEndPoints.StatDTOs;

public class SingleStatInfo
{
    public byte Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int StatLevel { get; set; }
    public StatDetails StatLevelInfo { get; set; }
}
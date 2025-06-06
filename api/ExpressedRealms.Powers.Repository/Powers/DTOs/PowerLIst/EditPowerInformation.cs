namespace ExpressedRealms.Powers.Repository.Powers.DTOs;

public class EditPowerInformation
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<int> CategoryIds { get; set; }
    public string Description { get; set; }
    public string GameMechanicEffect { get; set; }
    public string Limitation { get; set; }
    public int PowerDurationId { get; set; }
    public int AreaOfEffectId { get; set; }
    public int PowerLevelId { get; set; }
    public int PowerActivationTypeId { get; set; }
    public string? Other { get; set; }
    public bool IsPowerUse { get; set; }
}

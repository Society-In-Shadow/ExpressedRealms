namespace ExpressedRealms.Powers.UseCases.CharacterPower.GetPickablePowers;

public class PowerPathReturnModel
{
    public required string Name { get; set; }
    public List<PowerReturnModel> Powers { get; set; } = new();
}
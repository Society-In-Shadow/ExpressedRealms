namespace ExpressedRealms.Powers.UseCases.CharacterPower.GetPickablePowers.ReturnModels;

public class PowerPathResponse
{
    public required string Name { get; set; }
    public List<PowerResponse> Powers { get; set; } = new();
}

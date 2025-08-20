namespace ExpressedRealms.Powers.UseCases.CharacterPower.GetPickablePowers.ReturnModels;

public class PrerequisiteDetailsResponse
{
    public int RequiredAmount { get; set; }
    public List<string> Powers { get; set; } = new();
}

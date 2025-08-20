namespace ExpressedRealms.Powers.UseCases.CharacterPower.GetPowers.ReturnModels;

public class PrerequisiteDetailsReturnModel
{
    public int RequiredAmount { get; set; }
    public List<string> Powers { get; set; } = new();
}

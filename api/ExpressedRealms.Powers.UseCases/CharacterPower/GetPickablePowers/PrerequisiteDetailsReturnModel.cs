namespace ExpressedRealms.Powers.UseCases.CharacterPower.GetPickablePowers;

public class PrerequisiteDetailsReturnModel
{
    public int RequiredAmount { get; set; }
    public List<string> Powers { get; set; }
}
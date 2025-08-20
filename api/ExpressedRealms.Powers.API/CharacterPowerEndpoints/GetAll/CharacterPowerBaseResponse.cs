using ExpressedRealms.Powers.UseCases.CharacterPower.GetPickablePowers.ReturnModels;

namespace ExpressedRealms.Powers.API.CharacterPowerEndpoints.GetAll;

public class CharacterPowerBaseResponse
{
    public List<PowerPathResponse> Powers { get; set; } = new();
}

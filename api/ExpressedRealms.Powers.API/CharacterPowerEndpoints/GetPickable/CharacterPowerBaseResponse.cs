using ExpressedRealms.Powers.UseCases.CharacterPower.GetPickablePowers.ReturnModels;

namespace ExpressedRealms.Powers.API.CharacterPowerEndpoints.GetPickable;

public class CharacterPickablePowerBaseResponse
{
    public List<PowerPathResponse> Powers { get; set; } = new();
}

using ExpressedRealms.Powers.UseCases.CharacterPower.GetPowers.ReturnModels;

namespace ExpressedRealms.Powers.API.CharacterPowerEndpoints.GetAll.Responses;

public class DetailedInformationResponse
{
    public DetailedInformationResponse(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public DetailedInformationResponse(DetailedInformationReturnModel dto)
    {
        Name = dto.Name;
        Description = dto.Description;
    }
    
    public DetailedInformationResponse(UseCases.CharacterPower.GetPickablePowers.ReturnModels.DetailedInformationReturnModel dto)
    {
        Name = dto.Name;
        Description = dto.Description;
    }

    public string Name { get; set; }
    public string Description { get; set; }
}

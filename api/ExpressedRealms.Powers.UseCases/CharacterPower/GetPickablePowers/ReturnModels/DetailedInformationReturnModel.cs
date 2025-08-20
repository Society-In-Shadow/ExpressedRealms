using ExpressedRealms.Powers.Repository.Powers.DTOs.PowerList;

namespace ExpressedRealms.Powers.UseCases.CharacterPower.GetPickablePowers.ReturnModels;

public class DetailedInformationReturnModel
{
    public DetailedInformationReturnModel(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public DetailedInformationReturnModel(DetailedInformation dto)
    {
        Name = dto.Name;
        Description = dto.Description;
    }

    public string Name { get; set; }
    public string Description { get; set; }
}

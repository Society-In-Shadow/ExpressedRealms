namespace ExpressedRealms.Powers.UseCases.CharacterPower.GetPickablePowers;

public class DetailedInformation
{
    public DetailedInformation(string name, string description)
    {
        Name = name;
        Description = description;
    }
    
    public DetailedInformation(Repository.Powers.DTOs.PowerList.DetailedInformation dto)
    {
        Name = dto.Name;
        Description = dto.Description;
    }

    public string Name { get; set; }
    public string Description { get; set; }
}
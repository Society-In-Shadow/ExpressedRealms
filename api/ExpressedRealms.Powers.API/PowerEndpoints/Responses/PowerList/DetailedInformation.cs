namespace ExpressedRealms.Powers.API.PowerEndpoints.Responses.PowerList;

public class DetailedInformation
{
    public DetailedInformation(Repository.Powers.DTOs.PowerList.DetailedInformation dto)
    {
        Name = dto.Name;
        Description = dto.Description;
    }

    public string Name { get; set; }
    public string Description { get; set; }
}

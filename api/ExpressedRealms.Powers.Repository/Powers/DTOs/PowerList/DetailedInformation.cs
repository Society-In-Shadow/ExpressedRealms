namespace ExpressedRealms.Powers.Repository.Powers.DTOs.PowerList;

public class DetailedInformation
{
    public DetailedInformation(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public string Name { get; set; }
    public string Description { get; set; }
}

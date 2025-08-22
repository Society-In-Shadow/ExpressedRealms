namespace ExpressedRealms.Powers.Repository.Powers.DTOs.PowerList;

public class DetailedInformation
{
    public DetailedInformation(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public DetailedInformation(int id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}

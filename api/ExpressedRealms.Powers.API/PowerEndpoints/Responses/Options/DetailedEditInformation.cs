namespace ExpressedRealms.Powers.API.PowerEndpoints.Responses.Options;

public class DetailedEditInformation
{
    public DetailedEditInformation(
        Repository.Powers.DTOs.Options.DetailedEditInformation editInformation
    )
    {
        Id = editInformation.Id;
        Name = editInformation.Name;
        Description = editInformation.Description;
    }

    public DetailedEditInformation() { }

    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
}

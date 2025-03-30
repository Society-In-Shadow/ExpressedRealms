using ExpressedRealms.DB.Models.Powers;

namespace ExpressedRealms.Server.EndPoints.PowerEndpoints.Responses.Options;

public class DetailedEditInformation
{
    public DetailedEditInformation(
        ExpressedRealms.Repositories.Powers.Powers.DTOs.Options.DetailedEditInformation editInformation
    )
    {
        Id = editInformation.Id;
        Name = editInformation.Name;
        Description = editInformation.Description;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}

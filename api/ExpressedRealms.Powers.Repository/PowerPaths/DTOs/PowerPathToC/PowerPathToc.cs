using ExpressedRealms.Powers.Repository.Powers.DTOs;

namespace ExpressedRealms.Powers.Repository.PowerPaths.DTOs.PowerPathToC;

public class PowerPathToc
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public List<PowerInformation> Powers { get; set; } = new();
}

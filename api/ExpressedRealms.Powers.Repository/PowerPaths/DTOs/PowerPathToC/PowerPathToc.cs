using ExpressedRealms.Powers.Repository.Powers.DTOs;

namespace ExpressedRealms.Powers.Repository.PowerPaths.DTOs.PowerPathToC;

public class PowerPathToc
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<PowerInformation> Powers { get; set; }
}

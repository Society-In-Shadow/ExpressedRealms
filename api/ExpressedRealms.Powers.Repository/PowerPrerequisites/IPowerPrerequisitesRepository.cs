using ExpressedRealms.DB.Models.Powers.PowerPrerequisitePowerSetup;
using ExpressedRealms.DB.Models.Powers.PowerPrerequisiteSetup;
using FluentResults;

namespace ExpressedRealms.Powers.Repository.PowerPrerequisites;

public interface IPowerPrerequisitesRepository
{
    Task<int> AddPrerequisite(PowerPrerequisite model);
    Task AddPrerequisitePowers(List<PowerPrerequisitePower> model);
    Task<PowerPrerequisite> GetPrerequisiteForEditingAsync(int id);
    Task UpdatePrerequisite(PowerPrerequisite model);
    Task RemovePrerequisitePowers(int prerequisiteId);
    Task<Result> DeletePrerequisite(int prerequisiteId);
}

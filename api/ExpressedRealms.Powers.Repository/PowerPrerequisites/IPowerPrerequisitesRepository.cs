using ExpressedRealms.DB.Models.Powers.PowerPrerequisitePowerSetup;
using ExpressedRealms.DB.Models.Powers.PowerPrerequisiteSetup;
using ExpressedRealms.Powers.Repository.PowerPrerequisites.DTOs.DeletePrerequisite;
using ExpressedRealms.Powers.Repository.PowerPrerequisites.EditPowerPrerequisite;
using FluentResults;

namespace ExpressedRealms.Powers.Repository.PowerPrerequisites;

public interface IPowerPrerequisitesRepository
{
    Task<int> AddPrerequisite(PowerPrerequisite model);
    Task AddPrerequisitePowers(List<PowerPrerequisitePower> model);
    Task<PowerPrerequisite> GetPrerequisiteForEditingAsync(int id);
    Task UpdatePrerequisite(PowerPrerequisite model);
    Task RemovePrerequisitePowers(int prerequisiteId);
    Task UpdatePrerequisitePowers(List<PowerPrerequisitePower> model);
    Task<Result> DeletePrerequisite(DeletePrerequisiteModel model);
}
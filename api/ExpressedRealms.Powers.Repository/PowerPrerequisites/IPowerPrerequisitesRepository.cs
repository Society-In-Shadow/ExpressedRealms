using ExpressedRealms.DB.Models.Powers.PowerPrerequisitePowerSetup;
using ExpressedRealms.DB.Models.Powers.PowerPrerequisiteSetup;
using ExpressedRealms.Powers.Repository.PowerPrerequisites.DTOs.DeletePrerequisite;
using ExpressedRealms.Powers.Repository.PowerPrerequisites.DTOs.EditPrerequisite;
using FluentResults;

namespace ExpressedRealms.Powers.Repository.PowerPrerequisites;

public interface IPowerPrerequisitesRepository
{
    Task<int> AddPrerequisite(PowerPrerequisite model);
    Task AddPrerequisitePowers(List<PowerPrerequisitePower> model);
    Task<Result> EditPrerequisite(EditPrerequisiteModel model);
    Task<Result> DeletePrerequisite(DeletePrerequisiteModel model);
}
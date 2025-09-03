using ExpressedRealms.DB.Models.Blessings.BlessingLevelSetup;
using ExpressedRealms.DB.Models.Blessings.BlessingSetup;

namespace ExpressedRealms.Blessings.Repository.Blessings;

public interface IBlessingRepository
{
    Task<List<Blessing>> GetAllBlessingsAndBlessingLevels();
    Task<bool> HasDuplicateName(string name);
    Task<int> CreateBlessingAsync(Blessing blessing);
    Task<Blessing> GetBlessingForEditing(int id);
    Task EditBlessingAsync(Blessing blessing);
    Task<bool> IsExistingBlessing(int id);
    Task<int> CreateBlessingLevelAsync(BlessingLevel blessingLevel);
    Task<bool> HasDuplicateLevelName(int blessingId, string name);
}

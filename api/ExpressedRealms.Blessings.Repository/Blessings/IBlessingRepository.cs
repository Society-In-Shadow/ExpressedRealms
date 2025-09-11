using ExpressedRealms.DB.Models.Blessings.BlessingLevelSetup;
using ExpressedRealms.DB.Models.Blessings.BlessingSetup;

namespace ExpressedRealms.Blessings.Repository.Blessings;

public interface IBlessingRepository
{
    Task<List<Blessing>> GetAllBlessingsAndBlessingLevels();
    Task<bool> HasDuplicateName(string name, int blessingId = 0);
    Task<int> CreateBlessingAsync(Blessing blessing);
    Task<Blessing> GetBlessingForEditing(int id);
    Task EditBlessingAsync(Blessing blessing);
    Task<bool> IsExistingBlessing(int id);
    Task<int> CreateBlessingLevelAsync(BlessingLevel blessingLevel);
    Task<bool> HasDuplicateLevelName(int blessingId, string name, int levelId = 0);
    Task<BlessingLevel> GetBlessingLevelForEditing(int blessingId, int id);
    Task EditBlessingLevelAsync(BlessingLevel blessing);
    Task<bool> IsExistingBlessingLevel(int blessingId, int id);
    Task<bool> BlessingLevelExists(int id);
    Task<BlessingLevel> GetBlessingLevel(int blessingLevelId);
}

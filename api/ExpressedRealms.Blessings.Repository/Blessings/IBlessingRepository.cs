using ExpressedRealms.DB.Models.Blessings.BlessingSetup;

namespace ExpressedRealms.Blessings.Repository.Blessings;

public interface IBlessingRepository
{
    Task<List<Blessing>> GetAllBlessingsAndBlessingLevels();
    Task<bool> HasDuplicateName(string name);
    Task<int> CreateBlessingAsync(Blessing blessing);
}

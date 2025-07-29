using ExpressedRealms.DB.Models.Blessings.BlessingSetup;

namespace ExpressedRealms.Blessings.Repository.Blessings;

public interface IBlessingRepository
{
    Task<List<Blessing>> GetAllBlessingsAndBlessingLevels();
}

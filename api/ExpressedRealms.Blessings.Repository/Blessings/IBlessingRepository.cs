using ExpressedRealms.DB.Models.Blessings.BlessingSetup;

namespace ExpressedRealms.Blessings.Repository.Blessings;

internal interface IBlessingRepository
{
    Task<List<Blessing>> GetAllBlessingsAndBlessingLevels();
}
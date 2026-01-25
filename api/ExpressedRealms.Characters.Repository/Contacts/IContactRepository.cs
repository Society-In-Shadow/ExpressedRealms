using ExpressedRealms.DB.Models.Contacts;
using ExpressedRealms.Shared;

namespace ExpressedRealms.Characters.Repository.Contacts;

public interface IContactRepository : IGenericRepository
{
    Task<int> CreateAsync(Contact contact);
    Task<bool> HasDuplicateName(int characterId, string name);
}

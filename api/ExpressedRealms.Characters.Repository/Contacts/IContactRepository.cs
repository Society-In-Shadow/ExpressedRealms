using ExpressedRealms.Characters.Repository.Contacts.Dtos;
using ExpressedRealms.DB.Models.Contacts;
using ExpressedRealms.Shared;

namespace ExpressedRealms.Characters.Repository.Contacts;

public interface IContactRepository : IGenericRepository
{
    Task<int> CreateAsync(Contact contact);
    Task<bool> HasDuplicateName(int characterId, string name);
    Task<bool> HasDuplicateName(int contactId, int characterId, string name);
    Task<Contact?> FindContactAsync(int id);
    Task<List<ContactListDto>> GetContactsForCharacter(int characterId);
}

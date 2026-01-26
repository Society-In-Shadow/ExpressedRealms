using ExpressedRealms.Characters.Repository.Contacts.Dtos;
using ExpressedRealms.DB;
using ExpressedRealms.DB.Helpers;
using ExpressedRealms.DB.Models.Contacts;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Characters.Repository.Contacts;

internal sealed class ContactRepository(
    ExpressedRealmsDbContext context,
    CancellationToken cancellationToken
) : IContactRepository
{
    public async Task<int> CreateAsync(Contact contact)
    {
        context.Contacts.Add(contact);
        await context.SaveChangesAsync(cancellationToken);
        return contact.Id;
    }

    public async Task<bool> HasDuplicateName(int characterId, string name)
    {
        return await context.Contacts.AnyAsync(x => x.CharacterId == characterId && x.Name == name);
    }

    public async Task<bool> HasDuplicateName(int contactId, int characterId, string name)
    {
        return await context.Contacts.AnyAsync(x =>
            x.CharacterId == characterId && x.Name == name && x.Id != contactId
        );
    }

    public async Task<Contact?> FindContactAsync(int id)
    {
        return await context.Contacts.FindAsync(id);
    }

    public async Task<List<ContactListDto>> GetContactsForCharacter(int characterId)
    {
        return await context
            .Contacts.AsNoTracking()
            .Where(x => x.CharacterId == characterId)
            .Select(x => new ContactListDto()
            {
                Id = x.Id,
                Name = x.Name,
                Knowledge = x.Knowledge.Name,
                IsApproved = x.IsApproved,
                UsesPerWeek = x.Frequency,
                KnowledgeLevel = $"{x.KnowledgeLevel.Name} ({x.KnowledgeLevel.Level})",
            })
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    public async Task EditAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        await context.CommonSaveChanges(entity, cancellationToken);
    }
}

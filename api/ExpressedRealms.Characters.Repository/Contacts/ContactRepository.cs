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

    public async Task EditAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        await context.CommonSaveChanges(entity, cancellationToken);
    }
}

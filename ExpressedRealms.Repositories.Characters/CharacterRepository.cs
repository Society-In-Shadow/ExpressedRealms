using ExpressedRealms.DB;
using ExpressedRealms.Repositories.Characters.DTOs;
using ExpressedRealms.Repositories.Characters.ExternalDependencies;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Repositories.Characters;

public class CharacterRepository(ExpressedRealmsDbContext context, IUserContext userContext, CancellationToken cancellationToken) : ICharacterRepository
{
    public async Task<List<CharacterListDTO>> GetCharactersAsync()
    {
        return await context
            .Characters.Where(x => x.Player.UserId == userContext.CurrentUserId())
            .Select(x => new CharacterListDTO()
            {
                Id = x.Id.ToString(),
                Name = x.Name,
                Background = x.Background,
                Expression = x.Expression.Name
            })
            .ToListAsync(cancellationToken);
    }

}
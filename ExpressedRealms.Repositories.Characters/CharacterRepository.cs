using ExpressedRealms.DB;
using ExpressedRealms.DB.Characters;
using ExpressedRealms.Repositories.Characters.DTOs;
using ExpressedRealms.Repositories.Characters.ExternalDependencies;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Repositories.Characters;

public class CharacterRepository(ExpressedRealmsDbContext context, IUserContext userContext, CancellationToken cancellationToken, AddCharacterDtoValidator addValidator) : ICharacterRepository
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

    public async Task<GetEditCharacterDto?> GetCharacterInfoAsync(int id)
    {
        return await context.Characters.AsNoTracking()
            .Where(x =>
                x.Id == id && x.Player.UserId == userContext.CurrentUserId()
            )
            .Select(x => new GetEditCharacterDto()
            {
                Name = x.Name,
                Background = x.Background,
                Expression = x.Expression.Name,
                FactionId = x.FactionId
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Result<int>> CreateCharacter(AddCharacterDto dto)
    {
        var result = await addValidator.ValidateAsync(dto, cancellationToken);
        if (!result.IsValid)
            return Result.Fail(new FluentValidationFailure(result.ToDictionary()));
        
        
        var playerId = await context
            .Players.Where(x => x.UserId == userContext.CurrentUserId())
            .Select(x => x.Id)
            .FirstAsync(cancellationToken);

        var character = new Character()
        {
            Name = dto.Name,
            Background = dto.Background,
            ExpressionId = dto.ExpressionId,
            FactionId = dto.FactionId
        };

        character.PlayerId = playerId;
        
        context.Characters.Add(character);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Ok(character.Id);
    }
}
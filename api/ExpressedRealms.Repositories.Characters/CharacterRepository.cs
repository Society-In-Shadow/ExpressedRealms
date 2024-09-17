using ExpressedRealms.DB;
using ExpressedRealms.DB.Characters;
using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.Repositories.Characters.DTOs;
using ExpressedRealms.Repositories.Characters.Enums;
using ExpressedRealms.Repositories.Shared.CommonFailureTypes;
using ExpressedRealms.Repositories.Shared.ExternalDependencies;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Repositories.Characters;

internal sealed class CharacterRepository(
    ExpressedRealmsDbContext context,
    IUserContext userContext,
    AddCharacterDtoValidator addValidator,
    EditCharacterDtoValidator editValidator,
    CancellationToken cancellationToken
) : ICharacterRepository
{
    public async Task<List<CharacterListDto>> GetCharactersAsync()
    {
        return await context
            .Characters.Where(x => x.Player.UserId == userContext.CurrentUserId())
            .Select(x => new CharacterListDto()
            {
                Id = x.Id.ToString(),
                Name = x.Name,
                Background = x.Background,
                Expression = x.Expression.Name
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<Result<GetEditCharacterDto>> GetCharacterInfoAsync(int id)
    {
        var character = await context
            .Characters.AsNoTracking()
            .Where(x => x.Id == id && x.Player.UserId == userContext.CurrentUserId())
            .Select(x => new GetEditCharacterDto()
            {
                Name = x.Name,
                Background = x.Background,
                Expression = x.Expression.Name,
                FactionId = x.FactionId
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (character is null)
            return Result.Fail(new NotFoundFailure("Character"));

        return Result.Ok(character);
    }

    public async Task<Result<int>> CreateCharacterAsync(AddCharacterDto dto)
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

    public async Task<Result> DeleteCharacterAsync(int id)
    {
        var character = await context
            .Characters.IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Id == id && x.Player.UserId == userContext.CurrentUserId());

        if (character is null)
            return Result.Fail(new NotFoundFailure("Character"));

        if (character.IsDeleted)
            return Result.Fail(new AlreadyDeletedFailure("Character"));

        character.SoftDelete();
        await context.SaveChangesAsync();

        return Result.Ok();
    }

    public async Task<Result> UpdateCharacterAsync(EditCharacterDto dto)
    {
        var result = await editValidator.ValidateAsync(dto, cancellationToken);
        if (!result.IsValid)
            return Result.Fail(new FluentValidationFailure(result.ToDictionary()));

        var character = await context.Characters.FirstOrDefaultAsync(
            x => x.Id == dto.Id && x.Player.UserId == userContext.CurrentUserId(),
            cancellationToken
        );

        if (character is null)
            return Result.Fail(new NotFoundFailure("Character"));

        if (dto.FactionId is not null)
        {
            var isFaction = await context.ExpressionSections.AnyAsync(
                x =>
                    x.ExpressionId == character.ExpressionId
                    && x.SectionTypeId == (int)ExpressionSectionType.FactionType
                    && x.Id == dto.FactionId,
                cancellationToken
            );

            if (!isFaction)
            {
                return Result.Fail(
                    new FluentValidationFailure(
                        new Dictionary<string, string[]>
                        {
                            { "FactionId", ["This is not a valid Faction Id."] }
                        }
                    )
                );
            }
        }
        

        character.Name = dto.Name;
        character.Background = dto.Background;
        character.FactionId = dto.FactionId;

        await context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}

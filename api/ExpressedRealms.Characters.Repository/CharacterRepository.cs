using ExpressedRealms.Characters.Repository.DTOs;
using ExpressedRealms.Characters.Repository.Enums;
using ExpressedRealms.Characters.Repository.Skills;
using ExpressedRealms.Characters.Repository.Xp;
using ExpressedRealms.DB;
using ExpressedRealms.DB.Characters;
using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.Repositories.Shared;
using ExpressedRealms.Repositories.Shared.CommonFailureTypes;
using ExpressedRealms.Repositories.Shared.ExternalDependencies;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Characters.Repository;

internal sealed class CharacterRepository(
    ExpressedRealmsDbContext context,
    IUserContext userContext,
    AddCharacterDtoValidator addValidator,
    EditCharacterDtoValidator editValidator,
    CancellationToken cancellationToken,
    ICharacterSkillRepository skillRepository,
    IXpRepository xpRepository
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
                Expression = x.Expression.Name,
                IsPrimaryCharacter = x.IsPrimaryCharacter,
                IsInCharacterCreate = x.IsInCharacterCreation,
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<List<PrimaryCharacterListDto>> GetPrimaryCharactersAsync()
    {
        return await context
            .Characters.Where(x => x.IsPrimaryCharacter)
            .Select(x => new PrimaryCharacterListDto()
            {
                Id = x.Id.ToString(),
                Name = x.Name,
                Expression = x.Expression.Name,
                Background = x.Background,
                PlayerName = x.Player.Name,
                AssignedXp = x.AssignedXp,
                PlayerNumber = x.PlayerNumber,
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<Result<GetEditCharacterDto>> GetCharacterInfoAsync(int id)
    {
        var query = await context.Characters.AsNoTracking().WithUserAccessAsync(userContext, id);

        var character = await query
            .Select(x => new GetEditCharacterDto()
            {
                Name = x.Name,
                Background = x.Background,
                Expression = x.Expression.Name,
                FactionId = x.FactionId,
                ExpressionId = x.ExpressionId,
                IsPrimaryCharacter = x.IsPrimaryCharacter,
                IsInCharacterCreation = x.IsInCharacterCreation,
                IsOwner = x.Player.UserId == userContext.CurrentUserId(),
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (character is null)
            return Result.Fail(new NotFoundFailure("Character"));

        return Result.Ok(character);
    }

    public async Task<CharacterStatusDto> GetCharacterState(int id)
    {
        var query = await context.Characters.AsNoTracking().WithUserAccessAsync(userContext, id);

        return await query
            .Select(x => new CharacterStatusDto()
            {
                IsPrimaryCharacter = x.IsPrimaryCharacter,
                IsInCharacterCreation = x.IsInCharacterCreation,
                AssignedXp = x.AssignedXp,
            })
            .FirstAsync(cancellationToken);
    }

    public Task<Character> GetCharacterForEdit(int characterId)
    {
        return context.Characters.FirstAsync(x => x.Id == characterId);
    }

    public async Task UpdateCharacter(Character user)
    {
        context.Characters.Update(user);
        await context.SaveChangesAsync(cancellationToken);
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
            FactionId = dto.FactionId,
            IsInCharacterCreation = true,
            AssignedXp = 37,
        };

        character.PlayerId = playerId;

        context.Characters.Add(character);

        await context.SaveChangesAsync(cancellationToken);

        await skillRepository.AddDefaultSkills(character.Id);
        await xpRepository.AddDefaultCharacterXpMappings(character.Id);

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
                            { "FactionId", ["This is not a valid Faction Id."] },
                        }
                    )
                );
            }
        }

        character.Name = dto.Name;
        character.Background = dto.Background;
        character.FactionId = dto.FactionId;
        character.IsPrimaryCharacter = dto.IsPrimaryCharacter;

        await context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }

    public async Task<bool> CharacterExistsAsync(int id)
    {
        var query = await context.Characters.AsNoTracking().WithUserAccessAsync(userContext, id);

        var character = await query.FirstOrDefaultAsync();

        return character is not null;
    }
}

using ExpressedRealms.Characters.Repository.DTOs;
using ExpressedRealms.Characters.Repository.Skills;
using ExpressedRealms.Characters.Repository.Xp;
using ExpressedRealms.DB;
using ExpressedRealms.DB.Characters;
using ExpressedRealms.DB.Helpers;
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
                Id = x.Id,
                Name = x.Name,
                Expression = x.Expression.Name,
                Background = x.Background,
                PlayerName = x.Player.Name,
                PlayerNumber = x.PlayerNumber,
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<CharacterInfo> GetCharacterInfoForCRB(int characterId)
    {
        var query = await context
            .Characters.AsNoTracking()
            .WithUserAccessAsync(userContext, characterId);
        return await query
            .Select(x => new CharacterInfo()
            {
                CharacterName = x.Name,
                PlayerNumber = x.PlayerNumber,
                PlayerName = x.Player.Name,
                Expression = x.Expression.Name,
                PrimaryProgressionName =
                    x.PrimaryProgressionId == null ? "" : x.PrimaryProgressionPath!.Name,
                SecondaryProgressionName =
                    x.SecondaryProgressionId == null ? "" : x.SecondaryProgressionPath!.Name,
            })
            .FirstAsync(cancellationToken);
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
                PrimaryProgressionId = x.PrimaryProgressionId,
                SecondaryProgressionId = x.SecondaryProgressionId,
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (character is null)
            return Result.Fail(new NotFoundFailure("Character"));

        return Result.Ok(character);
    }

    public async Task<Character?> FindCharacterAsync(int id)
    {
        return await context.Characters.FindAsync([id], cancellationToken);
    }

    public async Task<CharacterStatusDto> GetCharacterState(int id)
    {
        var query = await context.Characters.AsNoTracking().WithUserAccessAsync(userContext, id);

        return await query
            .Select(x => new CharacterStatusDto()
            {
                IsPrimaryCharacter = x.IsPrimaryCharacter,
                IsInCharacterCreation = x.IsInCharacterCreation,
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

    public async Task EditAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        await context.CommonSaveChanges(entity, cancellationToken);
    }

    public async Task<bool> CharacterExistsAsync(int id)
    {
        var query = await context.Characters.AsNoTracking().WithUserAccessAsync(userContext, id);

        var character = await query.FirstOrDefaultAsync();

        return character is not null;
    }

    public async Task<bool> CanUpdatePrimaryCharacterStatus(int id)
    {
        var hasAnyPrimary = await context.Characters.AnyAsync(x =>
            x.IsPrimaryCharacter && x.Player.UserId == userContext.CurrentUserId()
        );

        if (!hasAnyPrimary)
            return true;

        return await context.Characters.AnyAsync(x =>
            x.Id == id && x.IsPrimaryCharacter && x.Player.UserId == userContext.CurrentUserId()
        );
    }
}

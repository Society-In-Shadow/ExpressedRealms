using System.Data;
using ExpressedRealms.Characters.Repository.DTOs;
using ExpressedRealms.Characters.Repository.Skills;
using ExpressedRealms.Characters.Repository.Xp;
using ExpressedRealms.DB;
using ExpressedRealms.DB.Helpers;
using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Characters;
using ExpressedRealms.Expressions.Repository.Expressions;
using ExpressedRealms.Repositories.Shared;
using ExpressedRealms.Repositories.Shared.CommonFailureTypes;
using ExpressedRealms.Repositories.Shared.ExternalDependencies;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;

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
                IsRetired = x.IsRetired,
                IsInCharacterCreate = x.IsInCharacterCreation,
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<List<ArchetypeCharacterInfoDto>> GetArchetypesForExpression(int expressionId)
    {
        return await context
            .Characters.Where(x => x.Player.IsArchetypeAccount && x.ExpressionId == expressionId)
            .Select(x => new ArchetypeCharacterInfoDto()
            {
                Id = x.Id,
                Name = x.Name,
                Background = x.Background,
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExpressionExistsAsync(int id)
    {
        return await context.Expressions.AnyAsync(
            x =>
                x.Id == id
                && x.PublishStatusId == (int)PublishTypes.Published
                && x.ExpressionTypeId == 1,
            cancellationToken
        );
    }

    public Task<Guid> GetPlayerId(string currentUserId)
    {
        return context.Players.Where(x => x.UserId == currentUserId).Select(x => x.Id).FirstAsync();
    }

    public async Task<List<PrimaryCharacterListDto>> GetPrimaryCharactersAsync()
    {
        var activeEventId = await GetActiveEventId();

        var maxStagePerPlayer = await context
            .CheckinStageMappings.Where(x => x.Checkin.EventId == activeEventId)
            .GroupBy(x => x.Checkin.PlayerId)
            .Select(g => new
            {
                PlayerId = g.Key,
                MaxStage = g.OrderByDescending(x => x.CreatedAt)
                    .Select(x => x.CheckinStageId)
                    .First(),
            })
            .ToListAsync(cancellationToken);

        var players = await context
            .Characters.Where(x => x.IsPrimaryCharacter)
            .Select(x => new PrimaryCharacterListDto()
            {
                Id = x.Id,
                Name = x.Name,
                Expression = x.Expression.Name,
                PlayerId = x.PlayerId,
                PlayerName = x.Player.Name,
                PlayerNumber = x.Player.PlayerNumber ?? 0,
            })
            .ToListAsync(cancellationToken);

        foreach (var player in players)
        {
            player.PlayerStageId = maxStagePerPlayer
                .FirstOrDefault(p => p.PlayerId == player.PlayerId)
                ?.MaxStage;
        }

        return players;
    }

    private async Task<int?> GetActiveEventId()
    {
        var now = DateOnly.FromDateTime(DateTime.UtcNow);

        var eventId = await context
            .Events.AsNoTracking()
            .Where(x => x.IsPublished && x.StartDate <= now && x.EndDate >= now)
            .Select(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        return eventId == 0 ? null : eventId;
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
                PlayerNumber = x.Player.PlayerNumber ?? 0,
                PlayerName = x.Player.Name,
                Expression = x.Expression.Name,
                LookupId = x.Player.LookupId,
                PrimaryProgressionName =
                    x.PrimaryProgressionId == null ? "" : x.PrimaryProgressionPath!.Name,
                SecondaryProgressionName =
                    x.SecondaryProgressionId == null ? "" : x.SecondaryProgressionPath!.Name,
            })
            .FirstAsync(cancellationToken);
    }

    public async Task<int> GetCharacterExpressionId(int characterId)
    {
        var query = await context
            .Characters.AsNoTracking()
            .WithUserAccessAsync(userContext, characterId);
        return await query.Select(x => x.Expression.Id).FirstAsync(cancellationToken);
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
                IsRetired = x.IsRetired,
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

    public async Task<int> CopyCharacterAsync(
        int sourceCharacterId,
        Guid targetPlayerId,
        string characterName
    )
    {
        var newCharacterIdParam = new NpgsqlParameter("new_character_id", NpgsqlDbType.Integer)
        {
            Direction = ParameterDirection.InputOutput,
            Value = DBNull.Value,
        };

        await context.Database.ExecuteSqlRawAsync(
            "CALL copy_character_to_player_proc(@source_character_id, @target_player_id, @character_name, @new_character_id)",
            new NpgsqlParameter("source_character_id", sourceCharacterId),
            new NpgsqlParameter("target_player_id", targetPlayerId),
            new NpgsqlParameter("character_name", characterName),
            newCharacterIdParam
        );

        return (int)newCharacterIdParam.Value;
    }

    public async Task UpdateCharacter(Character user)
    {
        context.Characters.Update(user);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result<int>> CreateCharacterAsync(AddCharacterDto addCharacterDto)
    {
        var result = await addValidator.ValidateAsync(addCharacterDto, cancellationToken);
        if (!result.IsValid)
            return Result.Fail(new FluentValidationFailure(result.ToDictionary()));

        var playerId = await context
            .Players.Where(x => x.UserId == userContext.CurrentUserId())
            .Select(x => x.Id)
            .FirstAsync(cancellationToken);

        var character = new Character()
        {
            Name = addCharacterDto.Name,
            Background = addCharacterDto.Background,
            ExpressionId = addCharacterDto.ExpressionId,
            FactionId = addCharacterDto.FactionId,
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

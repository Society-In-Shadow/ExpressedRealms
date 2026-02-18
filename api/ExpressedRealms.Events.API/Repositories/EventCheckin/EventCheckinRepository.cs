using ExpressedRealms.DB;
using ExpressedRealms.DB.Characters.AssignedXp.AssignedXpMappingModels;
using ExpressedRealms.DB.Helpers;
using ExpressedRealms.DB.Models.Checkins.CheckinQuestionResponseSetup;
using ExpressedRealms.DB.Models.Checkins.CheckinSetup;
using ExpressedRealms.DB.Models.Checkins.CheckinStageMappingSetup;
using ExpressedRealms.Events.API.Repositories.EventCheckin.Dtos;
using ExpressedRealms.Repositories.Shared.ExternalDependencies;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Events.API.Repositories.EventCheckin;

internal sealed class EventCheckinRepository(
    ExpressedRealmsDbContext context,
    IUserContext userContext,
    CancellationToken cancellationToken
) : IEventCheckinRepository
{
    public async Task<int> CreateCheckinAsync(Checkin checkin)
    {
        context.Checkins.Add(checkin);
        await context.SaveChangesAsync(cancellationToken);
        return checkin.Id;
    }

    public async Task<Checkin?> GetCheckinAsync(int eventId, Guid playerId)
    {
        return await context
            .Checkins.AsNoTracking()
            .Where(x => x.EventId == eventId && x.PlayerId == playerId)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<CheckinQuestionResponse>> GetAnsweredQuestions(int checkinId)
    {
        return await context
            .CheckinQuestionResponses.Where(x => x.CheckinId == checkinId)
            .ToListAsync(cancellationToken);
    }

    public Task<GoCheckinPrimaryCharacterInfoDto?> GetPrimaryCharacterInformation(Guid playerId)
    {
        return context
            .Characters.Where(x => x.PlayerId == playerId && x.IsPrimaryCharacter)
            .Select(x => new GoCheckinPrimaryCharacterInfoDto
            {
                CharacterId = x.Id,
                CharacterName = x.Name,
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<AssignedXpTypeDto?> GetAssignedXp(Guid playerId, int eventId)
    {
        List<int> validXpTypes = [2, 4, 5]; // checkin bonus, first time player, brought friend
        return await context
            .AssignedXpMappings.Where(x =>
                x.EventId == eventId
                && validXpTypes.Contains(x.AssignedXpTypeId)
                && x.PlayerId == playerId
            )
            .Select(x => new AssignedXpTypeDto() { Amount = x.Amount, TypeId = x.AssignedXpTypeId })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<CheckinQuestionResponse?> GetCheckinQuestionResponseAsync(
        int checkinId,
        int eventQuestionId
    )
    {
        return context.CheckinQuestionResponses.FirstOrDefaultAsync(
            x => x.CheckinId == checkinId && x.EventQuestionId == eventQuestionId,
            cancellationToken
        );
    }

    public async Task AddCheckinQuestionResponseAsync(
        CheckinQuestionResponse checkinQuestionResponse
    )
    {
        context.CheckinQuestionResponses.Add(checkinQuestionResponse);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<string> GetPlayerLookupId()
    {
        return await context
            .Players.AsNoTracking()
            .Where(x => x.UserId == userContext.CurrentUserId())
            .Select(x => x.LookupId!)
            .FirstAsync(cancellationToken);
    }

    public async Task<Guid> GetCurrentPlayerId()
    {
        return await context
            .Players.AsNoTracking()
            .Where(x => x.UserId == userContext.CurrentUserId())
            .Select(x => x.Id)
            .FirstAsync(cancellationToken);
    }

    public async Task<bool> CheckinIdExistsAsync(string id)
    {
        return await context
            .Players.AsNoTracking()
            .AnyAsync(x => x.LookupId == id, cancellationToken);
    }

    public async Task<int?> GetActiveEventId()
    {
        var now = DateOnly.FromDateTime(DateTime.UtcNow);

        var eventId = await context
            .Events.AsNoTracking()
            .Where(x => x.IsPublished && x.StartDate <= now && x.EndDate >= now)
            .Select(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        return eventId == 0 ? null : eventId;
    }

    public async Task<string> GetUserName(string lookupId)
    {
        return await context
            .Players.Where(x => x.LookupId == lookupId)
            .Select(x => x.Name)
            .FirstAsync(cancellationToken);
    }

    public async Task<Guid> GetPlayerId(string lookupId)
    {
        return await context
            .Players.Where(x => x.LookupId == lookupId)
            .Select(x => x.Id)
            .FirstAsync(cancellationToken);
    }

    public async Task<bool> IsFirstTimePlayer(string lookupId)
    {
        const int firstTimePlayerBonus = 4;
        return !await context
            .AssignedXpMappings.AsNoTracking()
            .AnyAsync(
                x => x.Player.LookupId == lookupId && x.AssignedXpTypeId == firstTimePlayerBonus,
                cancellationToken
            );
    }

    public int GetPlayerNumber(string lookupId)
    {
        // I hate this, but only way to do this in a single trip
        return context
            .Database.SqlQuery<int>(
                $"""
                    with updated as (
                        update "Players"
                        set player_number = nextval('player_number_sequence')
                        where lookup_id = {lookupId}
                        and (player_number is null or player_number = 0)
                        returning player_number
                    )
                    select player_number from updated
                    union all
                    select player_number from "Players"
                    where lookup_id = {lookupId}
                    limit 1
                """
            )
            .AsEnumerable()
            .First();
    }

    public async Task<bool> HasPreAssignedXpTypes(int eventId, Guid playerId)
    {
        List<int> validXpTypes = [2, 4, 5]; // checkin bonus, first time player, brought friend
        return await context.AssignedXpMappings.AnyAsync(
            x =>
                x.EventId == eventId
                && validXpTypes.Contains(x.AssignedXpTypeId)
                && x.PlayerId == playerId,
            cancellationToken
        );
    }

    public async Task<int> AddAssignedXpAsync(AssignedXpMapping entity)
    {
        context.AssignedXpMappings.Add(entity);
        await context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }

    public async Task EditAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        await context.CommonSaveChanges(entity, cancellationToken);
    }

    public async Task<int> CompleteStage(CheckinStageMapping mapping)
    {
        context.CheckinStageMappings.Add(mapping);
        await context.SaveChangesAsync(cancellationToken);
        return mapping.Id;
    }

    public async Task<List<CheckinStageMapping>> GetApprovedStages(int checkinId)
    {
        return await context
            .CheckinStageMappings.Where(x => x.CheckinId == checkinId)
            .ToListAsync(cancellationToken);
    }

    public async Task<BasicInfo?> GetCurrentStage(int checkinId)
    {
        return await context
            .CheckinStageMappings.Where(x => x.CheckinId == checkinId)
            .OrderByDescending(x => x.CheckinStageId)
            .Select(x => new BasicInfo { Id = x.CheckinStageId, Name = x.CheckinStage.Name })
            .FirstOrDefaultAsync(cancellationToken);
    }
}

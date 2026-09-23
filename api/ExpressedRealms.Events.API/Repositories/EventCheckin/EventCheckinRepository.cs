using System.Data;
using ExpressedRealms.DB;
using ExpressedRealms.DB.Helpers;
using ExpressedRealms.DB.Models.Characters.AssignedXP.AssignedXpMappingModels;
using ExpressedRealms.DB.Models.Characters.CharacterStorage.CharacterStorageModels;
using ExpressedRealms.DB.Models.Checkins.CheckinQuestionResponseSetup;
using ExpressedRealms.DB.Models.Checkins.CheckinSecondaryStatsSetup;
using ExpressedRealms.DB.Models.Checkins.CheckinSetup;
using ExpressedRealms.DB.Models.Checkins.CheckinStageMappingSetup;
using ExpressedRealms.DB.Models.Checkins.CheckinStageSetup;
using ExpressedRealms.DB.Models.Events.EventSetup;
using ExpressedRealms.DB.Models.Events.Questions.QuestionTypeSetup;
using ExpressedRealms.DB.Models.ModifierSystem.StatModifiers;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.PlayerSetup;
using ExpressedRealms.Events.API.Repositories.EventCheckin.Dtos;
using ExpressedRealms.Repositories.Shared.ExternalDependencies;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;

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

    public async Task<bool> DidBringFriendToCon(int checkinId)
    {
        return await context
            .CheckinQuestionResponses.Where(x =>
                x.CheckinId == checkinId
                && x.EventQuestion.QuestionTypeId == QuestionTypeEnum.BroughtNewPlayer
                && x.Response.Contains("Yes")
            )
            .AnyAsync(cancellationToken);
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
            .Select(x => x.LookupId)
            .FirstAsync(cancellationToken);
    }

    public async Task<string> GetPlayerLookupId(Guid playerId)
    {
        return await context
            .Players.Where(x => x.Id == playerId)
            .Select(x => x.LookupId)
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

    public async Task<Player> GetCurrentPlayerForEditingAsync()
    {
        return await context
            .Players.AsNoTracking()
            .Where(x => x.UserId == userContext.CurrentUserId())
            .FirstAsync(cancellationToken);
    }

    public async Task<string> GetCurrentPlayerName()
    {
        return await context
            .Players.AsNoTracking()
            .Where(x => x.UserId == userContext.CurrentUserId())
            .Select(x => x.Name)
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
        var eventId = await context
            .EventScheduleItems.FromSql(
                $@"
        SELECT event_schedule_items.*
        FROM public.event_schedule_items
        join public.events on events.id = event_schedule_items.event_id
        WHERE events.is_published = true
        AND (NOW() AT TIME ZONE time_zone_id)::date = event_schedule_items.date and events.is_deleted = false and event_schedule_items.is_deleted = false
        LIMIT 1
    "
            )
            .Select(x => x.EventId)
            .FirstOrDefaultAsync(cancellationToken);

        return eventId == 0 ? null : eventId;
    }

    public async Task<int?> GetExclusivePreCheckinEventId()
    {
        var eventId = await context
            .Events.FromSql(
                $@"
        SELECT e.*
        FROM public.events e
        JOIN (
            SELECT event_id, MIN(date) AS first_event_date
            FROM public.event_schedule_items
            WHERE is_deleted = false
            GROUP BY event_id 
        ) esi ON esi.event_id = e.id
        WHERE e.is_published = true
          AND e.is_deleted = false
          AND (NOW() AT TIME ZONE e.time_zone_id)::date
                      BETWEEN esi.first_event_date - 14
          AND esi.first_event_date - 1
        LIMIT 1
    "
            )
            .Select(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        return eventId == 0 ? null : eventId;
    }

    public async Task<int?> GetInclusivePreCheckinEventId()
    {
        var eventId = await context
            .Events.FromSql(
                $@"
SELECT e.*
FROM public.events e
JOIN (
    SELECT
        event_id,
        MIN(date) AS first_event_date
    FROM public.event_schedule_items
    WHERE is_deleted = false
    GROUP BY event_id
) first_day
    ON first_day.event_id = e.id
WHERE e.is_published = true
  AND e.is_deleted = false
  AND (
      (NOW() AT TIME ZONE e.time_zone_id)::date
          BETWEEN first_day.first_event_date - 14
              AND first_day.first_event_date - 1
      OR EXISTS (
          SELECT 1
          FROM public.event_schedule_items esi
          WHERE esi.event_id = e.id
            AND esi.is_deleted = false
            AND esi.date = (NOW() AT TIME ZONE e.time_zone_id)::date
      )
  )
LIMIT 1
    "
            )
            .Select(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        return eventId == 0 ? null : eventId;
    }

    public async Task<int> GetCurrentEventDay()
    {
        return await context
            .Database.SqlQuery<int>(
                $@"
            SELECT (
                (
                    (NOW() AT TIME ZONE e.time_zone_id)::date
                    - esi.first_event_date
                    + 1
                )::int
            ) AS ""Value""
            FROM public.events e
            JOIN (
                SELECT event_id, MIN(date) AS first_event_date
                FROM public.event_schedule_items
                WHERE is_deleted = false
                GROUP BY event_id
            ) esi ON esi.event_id = e.id
            WHERE e.is_published = true
              AND e.is_deleted = false
              AND (NOW() AT TIME ZONE e.time_zone_id)::date
                  BETWEEN esi.first_event_date AND e.end_date
            LIMIT 1
    "
            )
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<DateOnly> GetActiveEventStartDate()
    {
        return await context
            .Events.FromSql(
                $@"
        SELECT *
        FROM public.events
        WHERE is_published = true
        AND (NOW() AT TIME ZONE time_zone_id)::date BETWEEN start_date AND end_date and is_deleted = false
        LIMIT 1
    "
            )
            .Select(x => x.StartDate)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Event?> GetActiveEventInfoOrDefaultAsync()
    {
        return await context
            .Events.FromSql(
                $@"
        SELECT *
        FROM public.events
        WHERE is_published = true
        AND (NOW() AT TIME ZONE time_zone_id)::date BETWEEN start_date AND end_date and is_deleted = false
        LIMIT 1
    "
            )
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<string> GetPlayerName(string lookupId)
    {
        return await context
            .Players.Where(x => x.LookupId == lookupId)
            .Select(x => x.Name)
            .FirstAsync(cancellationToken);
    }

    public async Task<UserCrbEmailPreferenceDto> GetPlayerCrbEmailPreferenceWithPlayerNumber(
        Guid playerId
    )
    {
        return await context
            .Players.Where(x => x.Id == playerId)
            .Select(x => new UserCrbEmailPreferenceDto()
            {
                SendPickupCrbEmail = x.SendPickupCrbEmail,
                UserEmailAddress = x.User.Email!,
            })
            .FirstAsync(cancellationToken);
    }

    public async Task<UserCheckinPageDto> GetPlayerInfoForPlayerCheckinPage()
    {
        var currentEvent = await context
            .Events.FromSql(
                $@"
        SELECT *
        FROM public.events
        WHERE is_published = true
        AND (NOW() AT TIME ZONE time_zone_id)::date BETWEEN start_date AND end_date and is_deleted = false
        LIMIT 1
    "
            )
            .Select(x => new { x.Id, x.Name })
            .FirstOrDefaultAsync(cancellationToken);

        var info = await context
            .Players.Where(x => x.UserId == userContext.CurrentUserId())
            .Select(x => new UserCheckinPageDto()
            {
                LookupId = x.LookupId,
                SendPickupCrbEmail = x.SendPickupCrbEmail,
                CheckinId = x.Checkins.FirstOrDefault(y => y.EventId == currentEvent!.Id)!.Id,
            })
            .FirstAsync(cancellationToken);

        info.EventName = currentEvent!.Name;
        return info;
    }

    public Task<Player> GetPlayerAsync(string lookupId)
    {
        return context.Players.Where(x => x.LookupId == lookupId).FirstAsync();
    }

    public async Task<Guid> GetPlayerId(string lookupId)
    {
        return await context
            .Players.Where(x => x.LookupId == lookupId)
            .Select(x => x.Id)
            .FirstAsync(cancellationToken);
    }

    public async Task<Guid?> GetPlayerIdOrDefault(string lookupId)
    {
        return await context
            .Players.Where(x => x.LookupId == lookupId)
            .Select(x => (Guid?)x.Id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Guid?> GetPlayerIdFromCharacter(int characterId)
    {
        return await context
            .Characters.Where(x => x.Id == characterId)
            .Select(x => (Guid?)x.PlayerId)
            .FirstOrDefaultAsync(cancellationToken);
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
                        update players
                        set player_number = nextval('player_number_sequence')
                        where lookup_id = {lookupId}
                        and (player_number is null or player_number = 0)
                        returning player_number
                    )
                    select player_number from updated
                    union all
                    select player_number from players
                    where lookup_id = {lookupId}
                    limit 1
                """
            )
            .AsEnumerable()
            .First();
    }

    public async Task<bool> HasPreAssignedXpTypes(int eventId, Guid playerId)
    {
        List<int> validXpTypes =
        [
            AssignedXpTypeEnum.CheckinBonus,
            AssignedXpTypeEnum.FirstTimePlayerXp,
            AssignedXpTypeEnum.BroughtNewPlayerXp,
            AssignedXpTypeEnum.BoughtInitialCharacterStorage,
        ];
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

    public async Task<bool> GetStageStatus(int checkinId, CheckinStageEnum stageId)
    {
        return await context.CheckinStageMappings.AnyAsync(
            x => x.CheckinId == checkinId && x.CheckinStageId == stageId.Value,
            cancellationToken
        );
    }

    public async Task AddUpdateSecondaryStats(CheckinSecondaryStat checkinSecondaryStat)
    {
        var existing = await context.CheckinSecondaryStats.FirstOrDefaultAsync(x =>
            x.CheckinId == checkinSecondaryStat.CheckinId
        );

        if (existing is null)
        {
            await context.CheckinSecondaryStats.AddAsync(checkinSecondaryStat);
        }
        else
        {
            existing.Vitality = checkinSecondaryStat.Vitality;
            existing.Health = checkinSecondaryStat.Health;
            existing.Blood = checkinSecondaryStat.Blood;
            existing.Rwp = checkinSecondaryStat.Rwp;
            existing.Psyche = checkinSecondaryStat.Psyche;
            existing.Mortis = checkinSecondaryStat.Mortis;
            existing.Mana = checkinSecondaryStat.Mana;
            existing.Chi = checkinSecondaryStat.Chi;
            existing.Essence = checkinSecondaryStat.Essence;
            existing.Noumenon = checkinSecondaryStat.Noumenon;

            context.CheckinSecondaryStats.Update(existing);
        }

        await context.SaveChangesAsync();
    }

    public async Task<CheckinSecondaryStat?> GetSecondaryProficiencies(int checkinId)
    {
        return await context.CheckinSecondaryStats.FirstOrDefaultAsync(
            x => x.CheckinId == checkinId,
            cancellationToken
        );
    }

    public async Task<int> CreatePrimaryCharacterArchiveAsync(Guid targetPlayerId)
    {
        var characterInfo = await context
            .Characters.Where(x => x.PlayerId == targetPlayerId && x.IsPrimaryCharacter)
            .Select(x => new { x.Id, x.Name })
            .FirstAsync();

        // NOTE: There is a copy of this in Event Repository - There was a circular dependency loop
        var newCharacterIdParam = new NpgsqlParameter("new_character_id", NpgsqlDbType.Integer)
        {
            Direction = ParameterDirection.InputOutput,
            Value = DBNull.Value,
        };

        await context.Database.ExecuteSqlRawAsync(
            "CALL copy_character_to_player_proc(@p_source_character_id, @p_target_player_id, @p_character_name, @new_character_id)",
            new NpgsqlParameter("p_source_character_id", characterInfo.Id),
            new NpgsqlParameter("p_target_player_id", targetPlayerId),
            new NpgsqlParameter("p_character_name", characterInfo.Name),
            newCharacterIdParam
        );

        return (int)newCharacterIdParam.Value;
    }

    public Task<CharacterStorageInfo?> GetCharacterStorageInfo(Guid playerId, int eventId)
    {
        return context
            .CharacterStorageInfos.Where(x => x.PlayerId == playerId && x.EventId == eventId)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<int> AddCharacterStorageInfo(CharacterStorageInfo characterStorageInfo)
    {
        context.CharacterStorageInfos.Add(characterStorageInfo);
        await context.SaveChangesAsync(cancellationToken);
        return characterStorageInfo.Id;
    }

    public Task<List<CharacterStorageOptin>> GetCharacterStorageUsersForEvent(int activeEventId)
    {
        return context
            .CharacterStorageInfos.Where(x => x.EventId == activeEventId && x.OptedIn)
            .Select(x => new CharacterStorageOptin()
            {
                Id = x.Id,
                Timestamp = x.Timestamp,
                PlayerName = $"{x.Player.Name} ({x.Player.PlayerNumber})",
                ApproverName = $"{x.CollectorPlayer.Name} ({x.CollectorPlayer.PlayerNumber})",
                Amount = x.Amount,
            })
            .ToListAsync(cancellationToken);
    }

    public Task<bool> PlayerHasCharacterStorage(Guid playerId)
    {
        return context
            .CharacterStorageInfos.Where(x => x.PlayerId == playerId)
            .OrderByDescending(x => x.Timestamp)
            .Select(x => x.OptedIn)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<CheckinStageMapping>> GetActiveApprovedStages(int checkinId)
    {
        var activeList = new List<CheckinStageMapping>();
        var approvedStages = await GetApprovedStages(checkinId);

        var latestReapprovedStage = approvedStages
            .Where(x => x.CheckinStageId == CheckinStageEnum.PlayerNeedsReapproval.Value)
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefault();

        if (latestReapprovedStage is not null)
        {
            // Effectively keep the initial approval stages, then keep the reapprove stage and anything after that
            // Treat that list as the Canon List
            var stagesThatCannotBeReapproved = new[]
            {
                CheckinStageEnum.AgeCheckApproval.Value,
                CheckinStageEnum.EventQuestionsCheck.Value,
                CheckinStageEnum.CharacterStorageQuestion.Value,
                CheckinStageEnum.AssignedXpCheck.Value,
                CheckinStageEnum.ShqApproval.Value,
            };

            activeList.AddRange(
                approvedStages.Where(x => stagesThatCannotBeReapproved.Contains(x.CheckinStageId))
            );
            activeList.AddRange(
                approvedStages
                    .Where(x => x.CreatedAt >= latestReapprovedStage.CreatedAt)
                    .OrderByDescending(x => x.CreatedAt)
                    .ToList()
            );
        }
        else
        {
            activeList.AddRange(approvedStages);
        }

        return activeList.ToList();
    }
}

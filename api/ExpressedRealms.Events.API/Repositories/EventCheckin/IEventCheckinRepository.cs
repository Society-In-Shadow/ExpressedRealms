using ExpressedRealms.DB.Characters.AssignedXp.AssignedXpMappingModels;
using ExpressedRealms.DB.Models.Checkins.CheckinQuestionResponseSetup;
using ExpressedRealms.DB.Models.Checkins.CheckinSetup;
using ExpressedRealms.DB.Models.Checkins.CheckinStageMappingSetup;
using ExpressedRealms.Events.API.Repositories.EventCheckin.Dtos;
using ExpressedRealms.Shared;

namespace ExpressedRealms.Events.API.Repositories.EventCheckin;

public interface IEventCheckinRepository : IGenericRepository
{
    Task<string> GetPlayerLookupId();
    Task<int?> GetActiveEventId();
    Task<bool> CheckinIdExistsAsync(string id);
    Task<string> GetUserName(string lookupId);
    Task<bool> IsFirstTimePlayer(string lookupId);
    int GetPlayerNumber(string lookupId);
    Task<Guid> GetPlayerId(string lookupId);
    Task<int> CreateCheckinAsync(Checkin checkin);
    Task<Checkin?> GetCheckinAsync(int eventId, Guid playerId);
    Task<List<CheckinQuestionResponse>> GetAnsweredQuestions(int checkinId);
    Task<GoCheckinPrimaryCharacterInfoDto?> GetPrimaryCharacterInformation(Guid playerId);
    Task<AssignedXpTypeDto?> GetAssignedXp(Guid playerId, int eventId);
    Task<CheckinQuestionResponse?> GetCheckinQuestionResponseAsync(
        int checkinId,
        int eventQuestionId
    );
    Task AddCheckinQuestionResponseAsync(CheckinQuestionResponse checkinQuestionResponse);
    Task<int> AddAssignedXpAsync(AssignedXpMapping entity);
    Task<bool> HasPreAssignedXpTypes(int eventId, Guid playerId);
    Task<int> CompleteStage(CheckinStageMapping mapping);
    Task<List<CheckinStageMapping>> GetApprovedStages(int checkinId);
}

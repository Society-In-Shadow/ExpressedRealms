using ExpressedRealms.DB.Models.Checkins.CheckinQuestionResponseSetup;
using ExpressedRealms.DB.Models.Checkins.CheckinSetup;
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
    Task<int?> GetAssignedXp(Guid playerId, int eventId);
    Task<CheckinQuestionResponse?> GetCheckinQuestionResponseAsync(int eventId, int eventQuestionId);
    Task AddCheckinQuestionResponseAsync(CheckinQuestionResponse checkinQuestionResponse);
}

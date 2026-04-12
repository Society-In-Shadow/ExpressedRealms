using ExpressedRealms.Admin.Repository.DTOs;
using ExpressedRealms.Admin.Repository.Users.Dtos;
using ExpressedRealms.DB.Shared;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.PlayerSetup;

namespace ExpressedRealms.Admin.Repository;

public interface IUsersRepository
{
    Task<List<UserListDto>> GetUsersAsync();
    Task<bool> UserExistsAsync(string userId);
    Task<List<GenericListDto<string>>> GetUserSummaryAsync();
    Task<bool> PlayerNumberExists(int playerNumber);
    Task<PlayerBasicInfoDto> GetPlayerBasicInfoAsync(Guid id);
    Task<Player?> GetPlayerByUserIdForEditing(string userId);

    Task EditAsync<TEntity>(TEntity entity)
        where TEntity : class;
}

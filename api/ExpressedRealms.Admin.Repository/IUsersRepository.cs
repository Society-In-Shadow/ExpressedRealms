using ExpressedRealms.Admin.Repository.DTOs;
using ExpressedRealms.DB.Shared;

namespace ExpressedRealms.Admin.Repository;

public interface IUsersRepository
{
    Task<List<UserListDto>> GetUsersAsync();
    Task<bool> UserExistsAsync(string userId);
    Task<List<GenericListDto<string>>> GetUserSummaryAsync();
}

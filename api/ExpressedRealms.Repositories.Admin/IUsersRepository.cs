using ExpressedRealms.Repositories.Admin.DTOs;

namespace ExpressedRealms.Repositories.Admin;

public interface IUsersRepository
{
    Task<List<UserListDto>> GetUsersAsync();
}

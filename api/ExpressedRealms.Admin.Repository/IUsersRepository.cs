using ExpressedRealms.Admin.Repository.DTOs;

namespace ExpressedRealms.Admin.Repository;

public interface IUsersRepository
{
    Task<List<UserListDto>> GetUsersAsync();
}

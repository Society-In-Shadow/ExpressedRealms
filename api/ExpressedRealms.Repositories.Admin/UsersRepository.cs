using ExpressedRealms.DB;
using ExpressedRealms.Repositories.Admin.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Repositories.Admin;

internal sealed class UsersRepository(ExpressedRealmsDbContext context) : IUsersRepository
{
    public async Task<List<UserListDto>> GetUsersAsync()
    {
        return await context
            .Players.AsNoTracking()
            .Select(x => new UserListDto()
            {
                Id = x.Id,
                Email = x.User.Email,
                Username = x.Name,
            })
            .ToListAsync();
    }
}

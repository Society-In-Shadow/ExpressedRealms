using ExpressedRealms.DB;
using ExpressedRealms.Repositories.Admin.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Repositories.Admin;

internal sealed class UsersRepository(ExpressedRealmsDbContext context) : IUsersRepository
{
    public async Task<List<UserListDto>> GetUsersAsync()
    {
        var userRoles = await context.UserRoles.AsNoTracking().ToListAsync();
        var roles = await context.Roles.AsNoTracking().ToListAsync();

        var players = await context
            .Users.AsNoTracking()
            .Select(x => new UserListDto()
            {
                Id = x.Id,
                Email = x.Email,
                Username =
                    x.Player != null && x.Player.Name != null
                        ? x.Player.Name
                        : "Name hasn't been set yet.",
                IsDisabled = x.LockoutEnd.HasValue && x.LockoutEnd == DateTimeOffset.MaxValue,
                LockedOut = x.LockoutEnd.HasValue && x.LockoutEnd >= DateTimeOffset.UtcNow,
                LockOutExpires = x.LockoutEnd,
            })
            .ToListAsync();

        foreach (var player in players)
        {
            player.Roles = userRoles
                .Where(x => x.UserId == player.Id)
                .Select(x => roles.First(y => y.Id == x.RoleId).Name)
                .ToList();
        }

        return players;
    }
}

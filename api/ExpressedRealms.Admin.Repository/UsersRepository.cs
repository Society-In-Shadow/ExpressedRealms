using ExpressedRealms.Admin.Repository.DTOs;
using ExpressedRealms.DB;
using ExpressedRealms.DB.Shared;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.UserSetup;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Admin.Repository;

internal sealed class UsersRepository(
    ExpressedRealmsDbContext context,
    CancellationToken cancellationToken
) : IUsersRepository
{
    public async Task<List<UserListDto>> GetUsersAsync()
    {
        var userRoles = await context.UserRoles.AsNoTracking().ToListAsync();
        var roles = await context.Roles.AsNoTracking().ToListAsync();
        var currentDateTime = DateOnly.FromDateTime(DateTime.UtcNow);
        var players = await context
            .Users.AsNoTracking()
            .Select(x => new UserListDto()
            {
                Id = x.Id,
                Email = x.Email!,
                EmailConfirmed = x.EmailConfirmed,
                Username =
                    x.Player != null && x.Player!.Name != ""
                        ? x.Player.Name
                        : "Name hasn't been set yet.",
                IsDisabled = x.LockoutEnd.HasValue && x.LockoutEnd == DateTimeOffset.MaxValue,
                LockedOut = x.LockoutEnd.HasValue && x.LockoutEnd >= DateTimeOffset.UtcNow,
                LockOutExpires = x.LockoutEnd,
                Roles = x.UserRoleMappings
                    .Where(y => !y.ExpireDate.HasValue || y.ExpireDate >= currentDateTime)
                    .Select(y => new RoleInfoDto(){ Name = y.Role.Name, ExpirationDate = y.ExpireDate }).ToList(),
            })
            .ToListAsync();

        foreach (var player in players)
        {
            player.LegacyRoles = userRoles
                .Where(x => x.UserId == player.Id)
                .Select(x => roles.First(y => y.Id == x.RoleId).Name)
                .ToList();
        }

        return players;
    }

    public async Task<bool> UserExistsAsync(string userId)
    {
        return await context.Users.AnyAsync(x => x.Id == userId);
    }

    public async Task<List<GenericListDto<string>>> GetUserSummaryAsync()
    {
        return await context
            .Set<User>()
            .Select(x => new GenericListDto<string>()
            {
                Id = x.Id,
                Name = (x.Player != null ? x.Player.Name : "No Name Set") + " (" + x.Email + ")",
                Description = null,
            })
            .ToListAsync(cancellationToken);
    }
}

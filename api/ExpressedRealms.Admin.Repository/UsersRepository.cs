using ExpressedRealms.Admin.Repository.DTOs;
using ExpressedRealms.Admin.Repository.Users.Dtos;
using ExpressedRealms.DB;
using ExpressedRealms.DB.Helpers;
using ExpressedRealms.DB.Shared;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.PlayerSetup;
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
        var currentDateTime = DateOnly.FromDateTime(DateTime.UtcNow);

        var players = await context
            .Users.AsNoTracking()
            .Select(x => new UserListDto()
            {
                Id = x.Id,
                Email = x.Email!,
                EmailConfirmed = x.EmailConfirmed,
                Username =
                    x.Player != null
                        ? $"{x.Player.Name} ({(x.Player.PlayerNumber ?? 0):D3})"
                        : "Name hasn't been set yet.",
                IsDisabled = x.LockoutEnd.HasValue && x.LockoutEnd == DateTimeOffset.MaxValue,
                LockedOut = x.LockoutEnd.HasValue && x.LockoutEnd >= DateTimeOffset.UtcNow,
                LockOutExpires = x.LockoutEnd,
                Roles = x
                    .UserRoleMappings.Where(y =>
                        !y.ExpireDate.HasValue || y.ExpireDate >= currentDateTime
                    )
                    .Select(y => new RoleInfoDto()
                    {
                        Name = y.Role.Name,
                        ExpirationDate = y.ExpireDate,
                    })
                    .ToList(),
            })
            .ToListAsync();

        return players;
    }

    public async Task<bool> UserExistsAsync(string userId)
    {
        return await context.Users.AnyAsync(x => x.Id == userId);
    }

    public async Task<Player?> GetPlayerByUserIdForEditing(string userId)
    {
        return await context.Players.FirstAsync(x => x.UserId == userId, cancellationToken);
    }

    public Task<bool> PlayerNumberExists(int playerNumber)
    {
        if (playerNumber == 0)
            return Task.FromResult(false);
        // Will never be true as you cannot assign 0 to player number which is null
        return context.Players.AnyAsync(x => x.PlayerNumber == playerNumber, cancellationToken);
    }

    public async Task<bool> PlayerNumberExceedsMaxSequenceValue(int playerNumber)
    {
        var lastValue = await context
            .Database.SqlQuery<int>(
                $"""
                    SELECT last_value
                    FROM player_number_sequence
                """
            )
            .ToListAsync(cancellationToken);
        return lastValue[0] < playerNumber;
    }

    public Task<PlayerBasicInfoDto> GetPlayerBasicInfoAsync(Guid id)
    {
        return context
            .Players.AsNoTracking()
            .Where(x => x.UserId == id.ToString())
            .Select(x => new PlayerBasicInfoDto() { PlayerNumber = x.PlayerNumber })
            .FirstAsync(cancellationToken);
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

    public async Task EditAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        await context.CommonSaveChanges(entity, cancellationToken);
    }
}

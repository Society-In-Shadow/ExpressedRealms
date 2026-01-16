using System.Security.Claims;
using System.Text.Json;
using ExpressedRealms.Admin.Repository;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.UserSetup;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace ExpressedRealms.Admin.API.CustomClaimsPrincipleFactory;

public class ClaimStash(
    UserManager<User> userManager,
    IRolesRepository rolesRepository,
    IConnectionMultiplexer redis,
    ILogger<ClaimStash> logger)
{
    private record ClaimDto(string Type, string Value);
    public async Task CreateClaimsCache(User user, string nameIdentifier)
    {
        var permissions = await rolesRepository.GetPermissionKeysForUserAsync(user.Id);
        var roles = await userManager.GetRolesAsync(user);

        var claimsToStore = roles.Select(x => new ClaimDto(ClaimTypes.Role, x)).ToList();
        claimsToStore.AddRange(permissions.Select(x => new ClaimDto("custom_permission", x)).ToList());
        
        // Store in Redis with TTL equivalent to Default Cookie Length
        var db = redis.GetDatabase();
        var json = JsonSerializer.Serialize(claimsToStore);
        await db.StringSetAsync($"claims:{nameIdentifier}", json, TimeSpan.FromDays(14));
        logger.LogTrace($" -- Claim Cache was created");
    }
    
    public async Task<List<Claim>> GetClaimsFromCache(ClaimsPrincipal principal, string nameIdentifier)
    {
        logger.LogTrace($" -- Start Claim Cache retrieval for {nameIdentifier}");
        var db = redis.GetDatabase();
        var redisValue = await db.StringGetAsync($"claims:{nameIdentifier}");

        var hasCache = !redisValue.IsNullOrEmpty;
        if (hasCache)
        {
            logger.LogTrace($" -- Claim Cache was hit");
            await db.KeyExpireAsync($"claims:{nameIdentifier}", TimeSpan.FromDays(14));
            return await GetClaimsFromCache(redisValue);
        }
        
        logger.LogWarning($" -- Claim Cache was missed");
        
        var user = await userManager.GetUserAsync(principal);
        
        if (user == null || user is { LockoutEnabled: true, LockoutEnd: not null } && user.LockoutEnd > DateTime.UtcNow)
        {
            return new List<Claim>() { new Claim("KickUserOut", "KickUserOut") };
        }

        await CreateClaimsCache(user, nameIdentifier);
        
        return await GetClaimsFromCache(principal, nameIdentifier);
    }

    private static async Task<List<Claim>> GetClaimsFromCache(RedisValue redisValue)
    {
        var json = (string)redisValue!;
        var payload = JsonSerializer.Deserialize<List<ClaimDto>>(json);
        return payload.Select(x => new Claim(x.Type, x.Value)).ToList();
    }
}
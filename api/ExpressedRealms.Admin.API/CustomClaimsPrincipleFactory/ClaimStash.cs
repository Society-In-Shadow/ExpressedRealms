using System.Security.Claims;
using System.Text.Json;
using ExpressedRealms.Admin.Repository;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.UserSetup;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using StackExchange.Redis;

namespace ExpressedRealms.Admin.API.CustomClaimsPrincipleFactory;

public class ClaimStash(
    UserManager<User> userManager,
    IRolesRepository rolesRepository,
    IConnectionMultiplexer redis,
    ILogger<ClaimStash> logger)
{
    private static readonly AsyncRetryPolicy RedisRetryPolicy = Policy
        .Handle<RedisConnectionException>()
        .Or<TimeoutException>()
        .WaitAndRetryAsync(
            retryCount: 2,
            sleepDurationProvider: attempt => TimeSpan.FromMilliseconds(50 * attempt)
        );
    
    private record ClaimDto(string Type, string Value);
    
    public async Task CreateClaimsCache(User user, string nameIdentifier)
    {
        if (!redis.IsConnected)
        {
            logger.LogCritical("Redis connection is down, skipping initial claim cache creation");
            return;
        }

        var claimsToStore = await GetUserClaimsFromDatabase(user);

        // Store in Redis with TTL equivalent to Default Cookie Length
        var db = redis.GetDatabase();
        var json = JsonSerializer.Serialize(claimsToStore);

        try
        {
            await RedisRetryPolicy.ExecuteAsync(async () =>
            {
                await db.StringSetAsync($"claims:{nameIdentifier}", json, TimeSpan.FromDays(14));
            });

            logger.LogTrace("Claims cache successfully created for user {UserId}", user.Id);
        }
        catch (Exception ex)
        {
            // Fail-fast: Redis is down, log, and continue; DB will serve claims
            logger.LogWarning(ex, "Failed to cache claims for user {UserId}. Claims will be fetched from DB.", user.Id);
        }
    }

    public async Task<List<Claim>> GetClaimsFromCache(ClaimsPrincipal principal, string nameIdentifier)
    {
        var db = redis.GetDatabase();

        var redisDown = !redis.IsConnected;

        if (redisDown)
        {
            logger.LogCritical("Redis connection is down, auth claim population skipping directly to DB calls");
        }
        else
        {
            try
            {
                List<Claim>? cachedClaims = null;
                await RedisRetryPolicy.ExecuteAsync(async () =>
                {
                    var redisValue = await db.StringGetAsync($"claims:{nameIdentifier}");
                    var hasCache = !redisValue.IsNullOrEmpty;
                    if (hasCache)
                    {
                        logger.LogInformation($" -- Claim Cache was hit");
                        await db.KeyExpireAsync($"claims:{nameIdentifier}", TimeSpan.FromDays(14));
                        cachedClaims = TranslateCacheIntoClaims(redisValue);
                    }
                });
                
                if (cachedClaims != null) return cachedClaims;
                
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Failed to cache claims for user {UserId}. Claims will be fetched from DB.",
                    nameIdentifier);
                redisDown = true;
            }
        }

        logger.LogTrace(" -- Claim Cache was missed");
        
        // Before repopulating the cache, make sure the user can still be logged in
        var user = await userManager.GetUserAsync(principal);
        if (user == null || user is { LockoutEnabled: true, LockoutEnd: not null } && user.LockoutEnd > DateTime.UtcNow)
        {
            return new List<Claim>() { new Claim("KickUserOut", "KickUserOut") };
        }

        // If the entry expired, and the cache isn't down, recreate the cache
        if (!redisDown)
        {
            logger.LogInformation(" -- Claim Cache expired, recreating");
            await CreateClaimsCache(user, nameIdentifier);
            return await GetClaimsFromCache(principal, nameIdentifier);
        }

        // Treat Postgres as the cache and return the claims
        var claimsToStore = await GetUserClaimsFromDatabase(user);
        return claimsToStore.Select(x => new Claim(x.Type, x.Value)).ToList();
    }

    private async Task<List<ClaimDto>> GetUserClaimsFromDatabase(User user)
    {
        var permissions = await rolesRepository.GetPermissionKeysForUserAsync(user.Id);
        var roles = await userManager.GetRolesAsync(user);

        var claimsToStore = roles.Select(x => new ClaimDto(ClaimTypes.Role, x)).ToList();
        claimsToStore.AddRange(permissions.Select(x => new ClaimDto("custom_permission", x)).ToList());
        return claimsToStore;
    }
    
    private static List<Claim> TranslateCacheIntoClaims(RedisValue redisValue)
    {
        var json = (string)redisValue!;
        var payload = JsonSerializer.Deserialize<List<ClaimDto>>(json);
        return payload.Select(x => new Claim(x.Type, x.Value)).ToList();
    }
}
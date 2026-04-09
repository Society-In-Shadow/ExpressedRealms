using ExpressedRealms.DB;
using ExpressedRealms.Server.Shared.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Server.EndPoints.AuthEndpoints.UserEndpoint;

public static class GetUserEndpoint
{
    public static async Task<Ok<UserResponse>> ExecuteAsync(
        HttpContext httpContext,
        [FromServices] ExpressedRealmsDbContext dbContext
    )
    {
        if (!httpContext.User.Identity?.IsAuthenticated ?? false)
        {
            return TypedResults.Ok(new UserResponse() { UserInfo = null });
        }

        var userId = httpContext.User.GetUserId();
        var userInfo = await dbContext
            .Users.Where(x => x.Id == userId)
            .Select(x => new
            {
                EmailConfirmed = x.EmailConfirmed,
                Username = x.Player != null ? x.Player.Name : null,
                Email = x.Email,
            })
            .FirstAsync();

        var stateType = SetupState.Done;
        if (!userInfo.EmailConfirmed)
        {
            stateType = SetupState.UnconfirmedEmail;
        }
        else if (string.IsNullOrWhiteSpace(userInfo.Username))
        {
            stateType = SetupState.SetProfileName;
        }

        return TypedResults.Ok(
            new UserResponse()
            {
                UserInfo = new UserInfo()
                {
                    Name = userInfo.Username,
                    SetupState = stateType,
                    Email = userInfo.Email,
                },
            }
        );
    }
}

using ExpressedRealms.Admin.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Admin.API.AdminEndpoints.GetUsers;

public static class GetUsersEndpoint
{
    public static async Task<Ok<UserListResponse>> Execute(
        [FromServices] IUsersRepository repository
    )
    {
        var users = await repository.GetUsersAsync();

        return TypedResults.Ok(
            new UserListResponse()
            {
                Users = users
                    .Select(x => new UserListItem()
                    {
                        Id = x.Id,
                        Email = x.Email,
                        Username = x.Username,
                        Roles = x.Roles,
                        IsDisabled = x.IsDisabled,
                        EmailConfirmed = x.EmailConfirmed,
                        LockedOut = x.LockedOut,
                        LockedOutExpires = x.LockOutExpires,
                    })
                    .OrderBy(x => x.Email)
                    .ToList(),
            }
        );
    }
}

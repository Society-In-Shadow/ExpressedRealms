using System.Security.Claims;
using ExpressedRealms.Authentication;
using ExpressedRealms.DB;
using ExpressedRealms.Expressions.Repository.Expressions;
using ExpressedRealms.FeatureFlags;
using ExpressedRealms.FeatureFlags.FeatureClient;
using ExpressedRealms.Server.EndPoints.NavigationEndpoints.DTOs;
using ExpressedRealms.Server.EndPoints.NavigationEndpoints.Responses;
using ExpressedRealms.Server.Shared;
using ExpressedRealms.Server.Shared.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace ExpressedRealms.Server.EndPoints.PlayerEndpoints;

internal static class NavigationEndpoints
{
    internal static void AddNavigationEndpoints(this WebApplication app)
    {
        var endpointGroup = app.MapGroup("navMenu")
            .AddFluentValidationAutoValidation()
            .WithTags("Nav Menu");

        endpointGroup
            .MapGet(
                "/permissions",
                async Task<Ok<PermissionResponse>> (
                    HttpContext httpContext,
                    IExpressionRepository repository
                ) =>
                {
                    if (!httpContext.User.Identity?.IsAuthenticated ?? false)
                    {
                        return TypedResults.Ok(
                            new PermissionResponse { Roles = new List<string>() }
                        );
                    }

                    return TypedResults.Ok(
                        new PermissionResponse
                        {
                            Roles = httpContext
                                .User.Claims.Where(x => x.Type == ClaimTypes.Role)
                                .Select(x => x.Value)
                                .ToList(),
                        }
                    );
                }
            )
            .AllowAnonymous();

        endpointGroup
            .MapGet(
                "/featureFlags",
                async Task<Ok<FeatureFlagResponse>> (IFeatureToggleClient featureFlags) =>
                {
                    List<string> featureFlagList = new List<string>();
                    foreach (var featureFlag in ReleaseFlags.List)
                    {
                        if (await featureFlags.HasFeatureFlag(featureFlag))
                        {
                            featureFlagList.Add(featureFlag.Value);
                        }
                    }

                    return TypedResults.Ok(
                        new FeatureFlagResponse() { FeatureFlags = featureFlagList }
                    );
                }
            )
            .AllowAnonymous();

        endpointGroup
            .MapGet(
                "/content/",
                async Task<Ok<ExpressionMenuResponse>> (
                    HttpContext httpContext,
                    IExpressionRepository repository
                ) =>
                {
                    var navMenuItems = await repository.GetNavigationMenuItems();

                    var hasEditPolicy = await httpContext.UserHasPolicyAsync(
                        Policies.ExpressionEditorPolicy
                    );

                    var menuItems = navMenuItems
                        .Value.Select(x => new ExpressionMenuItem(x))
                        .ToList();

                    if (hasEditPolicy)
                    {
                        menuItems.AddRange(
                            [
                                new ExpressionMenuItem()
                                {
                                    Id = 0,
                                    Name = $"Add Expression",
                                    ShortDescription = $"Use this to add a new Expression",
                                    ExpressionTypeId = 1,
                                    NavMenuImage = "add",
                                    OrderIndex = 1000000000,
                                },
                                new ExpressionMenuItem()
                                {
                                    Id = 0,
                                    Name = $"Add Rule Book Section",
                                    ShortDescription = $"Use this to add a new Rule Book Section",
                                    NavMenuImage = "add",
                                    ExpressionTypeId = 13,
                                    OrderIndex = 1000000000,
                                },
                                new ExpressionMenuItem()
                                {
                                    Id = 0,
                                    Name = $"Add World Background Section",
                                    ShortDescription =
                                        $"Use this to add a new World Background Section",
                                    NavMenuImage = "add",
                                    ExpressionTypeId = 14,
                                    OrderIndex = 1000000000,
                                },
                            ]
                        );
                    }

                    var sorted = menuItems
                        .OrderBy(x => x.ExpressionTypeId)
                        .ThenBy(x => x.OrderIndex)
                        .ToList();

                    return TypedResults.Ok(
                        new ExpressionMenuResponse() { CanEdit = hasEditPolicy, MenuItems = sorted }
                    );
                }
            )
            .RequireAuthorization();

        endpointGroup
            .MapGet(
                "characters",
                [Authorize]
                async (ExpressedRealmsDbContext dbContext, HttpContext http) =>
                {
                    var characters = await dbContext
                        .Characters.Where(x => x.Player.UserId == http.User.GetUserId())
                        .OrderByDescending(x => x.IsPrimaryCharacter)
                        .ThenBy(x => x.Name)
                        .Select(x => new CharacterNavResponse(
                            x.Id,
                            x.Name,
                            x.Expression.Name,
                            x.IsPrimaryCharacter ? 1 : x.IsRetired ? 2 : 0
                        ))
                        .Take(6)
                        .ToListAsync();
                    
                    return TypedResults.Ok(characters);
                }
            )
            .WithDescription(
                "Returns primary character first if it exists, then an alphabetized list of characters.  Will only return 6 characters total."
            )
            .RequireAuthorization();
    }
}

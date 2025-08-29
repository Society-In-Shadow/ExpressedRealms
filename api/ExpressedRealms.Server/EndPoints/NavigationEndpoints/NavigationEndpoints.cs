using System.Security.Claims;
using ExpressedRealms.Authentication;
using ExpressedRealms.DB;
using ExpressedRealms.Expressions.Repository.Expressions;
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
            .WithTags("Nav Menu")
            .WithOpenApi();

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
                    foreach (var featureFlag in FeatureFlags.ReleaseFlags.List)
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
                "/content/{id}",
                async Task<Ok<ExpressionMenuResponse>> (
                    int id,
                    HttpContext httpContext,
                    IExpressionRepository repository
                ) =>
                {
                    var navMenuItems = await repository.GetNavigationMenuItems(id);

                    var hasEditPolicy = await httpContext.UserHasPolicyAsync(
                        Policies.ExpressionEditorPolicy
                    );

                    var menuItems = navMenuItems
                        .Value.Select(x => new ExpressionMenuItem(x))
                        .ToList();

                    var name = id switch
                    {
                        1 => "Expression",
                        13 => "Rule Book Section",
                        14 => "World Background Section",
                        _ => "Unknown"
                    };
                    
                    if (hasEditPolicy)
                    {
                        menuItems.Add(
                            new ExpressionMenuItem()
                            {
                                Id = 0,
                                Name = $"Add {name}",
                                ShortDescription = $"Use this to add a new {name}",
                                NavMenuImage = "pi-plus",
                            }
                        );
                    }

                    return TypedResults.Ok(
                        new ExpressionMenuResponse()
                        {
                            CanEdit = hasEditPolicy,
                            MenuItems = menuItems,
                        }
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
                        .Select(x => new CharacterNavResponse(
                            x.Id,
                            x.Name,
                            x.Expression.Name,
                            x.Background.Substring(0, 51) ?? ""
                        ))
                        .ToListAsync();

                    characters.ForEach(x =>
                    {
                        if (x.Background.Length > 50)
                        {
                            x.Background = x.Background.Substring(0, 50) + "...";
                        }
                    });

                    return TypedResults.Ok(characters);
                }
            )
            .WithSummary(
                "Returns a simplified version of the characters for display in the nav menu."
            )
            .WithDescription(
                "Returns the all characters with their name, expression, and a truncated background (50 characters + '...',  or less)."
            )
            .RequireAuthorization();
    }
}

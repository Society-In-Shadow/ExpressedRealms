using ExpressedRealms.Admin.API.AdminCharacterList.GetCharacterList;
using ExpressedRealms.Admin.API.AdminCharacterList.UpdateCharacterXp;
using ExpressedRealms.Authentication.PermissionCollection;
using ExpressedRealms.Authentication.PermissionCollection.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace ExpressedRealms.Admin.API.AdminCharacterList;

public static class AdminCharacterListEndpoints
{
    internal static void AddAdminCharacterListEndPoints(this WebApplication app)
    {
        var endpointGroup = app.MapGroup("admin")
            .AddFluentValidationAutoValidation()
            .WithTags("Admin Character List")
            .RequirePermission(Permissions.CharacterManagement.View);

        endpointGroup
            .MapGet("characters/", GetCharacterListEndpoint.Execute)
            .RequireAuthorization();

        endpointGroup
            .MapPut("characters/{characterId}/updateXp", UpdateCharacterXpEndpoint.Execute)
            .RequireAuthorization();
    }
}

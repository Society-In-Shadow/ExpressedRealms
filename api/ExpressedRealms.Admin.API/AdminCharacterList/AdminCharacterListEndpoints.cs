using ExpressedRealms.Admin.API.AdminCharacterList.GetCharacterList;
using ExpressedRealms.Admin.API.AdminCharacterList.UpdateCharacterXp;
using ExpressedRealms.Authentication;
using ExpressedRealms.Server.Shared;
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
            .RequirePolicyAuthorization(Policies.ManagePlayerCharacterList);

        endpointGroup
            .MapGet("characters/", GetCharacterListEndpoint.Execute)
            .RequireAuthorization();

        endpointGroup
            .MapPut("characters/{characterId}/updateXp", UpdateCharacterXpEndpoint.Execute)
            .RequireAuthorization();
    }
}

using ExpressedRealms.Authentication.PermissionCollection;
using ExpressedRealms.Authentication.PermissionCollection.Configuration;
using ExpressedRealms.Knowledges.API.GetKnowledgeTypes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace ExpressedRealms.Knowledges.API;

public static class KnowledgeTypeEndpoints
{
    internal static void AddKnowledgeTypesEndpoints(this WebApplication app)
    {
        var endpointGroup = app.MapGroup("knowledgetypes").WithTags("Knowledges");

        endpointGroup
            .MapGet("", GetKnowledgeTypesEndpoint.GetKnowledgeTypes)
            .RequirePermission(Permissions.Knowledges.View);
    }
}

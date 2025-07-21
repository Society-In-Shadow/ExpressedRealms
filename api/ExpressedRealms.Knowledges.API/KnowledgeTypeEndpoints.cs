using ExpressedRealms.Authentication;
using ExpressedRealms.Knowledges.API.GetKnowledgeTypes;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace ExpressedRealms.Knowledges.API;

public static class KnowledgeTypeEndpoints
{
    internal static void AddKnowledgeTypesEndpoints(this WebApplication app)
    {
        var endpointGroup = app.MapGroup("knowledgetypes")
            .AddFluentValidationAutoValidation()
            .WithTags("Knowledges")
            .WithOpenApi();

        endpointGroup
            .MapGet("", GetKnowledgeTypesEndpoint.GetKnowledgeTypes)
            .RequirePolicyAuthorization(Policies.ManageKnowledges);
    }
}

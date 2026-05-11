using ExpressedRealms.Authentication.PermissionCollection;
using ExpressedRealms.Authentication.PermissionCollection.Configuration;
using ExpressedRealms.Knowledges.API.CreateKnowledge;
using ExpressedRealms.Knowledges.API.DeleteKnowledge;
using ExpressedRealms.Knowledges.API.EditKnowledge;
using ExpressedRealms.Knowledges.API.GetAllKnowledges;
using ExpressedRealms.Knowledges.API.GetKnowledge;
using ExpressedRealms.Knowledges.API.GetKnowledgeSummary;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace ExpressedRealms.Knowledges.API;

internal static class KnowledgeEndpoints
{
    internal static void AddKnowledgeEndpoints(this WebApplication app)
    {
        var endpointGroup = app.MapGroup("knowledges").WithTags("Knowledges");

        endpointGroup.MapGet("", GetKnowledgesEndpoint.GetKnowledges);

        endpointGroup.MapGet("summary", GetKnowledgeSummaryEndpoint.ExecuteAsync);

        endpointGroup
            .MapGet("{id}", GetKnowledgeEndpoint.GetKnowledge)
            .RequirePermission(Permissions.Knowledges.View);

        endpointGroup
            .MapPost("", CreateKnowledgeEndpoint.CreateKnowledge)
            .RequirePermission(Permissions.Knowledges.Create);

        endpointGroup
            .MapPut("{id}", EditKnowledgeEndpoint.EditKnowledges)
            .RequirePermission(Permissions.Knowledges.Edit);

        endpointGroup
            .MapDelete("{id}", DeleteKnowledgeEndpoint.DeleteKnowledge)
            .RequirePermission(Permissions.Knowledges.Delete);
    }
}

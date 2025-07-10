﻿using ExpressedRealms.Authentication;
using ExpressedRealms.FeatureFlags;
using ExpressedRealms.Knowledges.API.CreateKnowledge;
using ExpressedRealms.Knowledges.API.DeleteKnowledge;
using ExpressedRealms.Knowledges.API.EditKnowledge;
using ExpressedRealms.Knowledges.API.GetAllKnowledges;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace ExpressedRealms.Knowledges.API;

internal static class KnowledgeEndpoints
{
    internal static void AddKnowledgeEndpoints(this WebApplication app)
    {
        var endpointGroup = app.MapGroup("knowledges")
            .AddFluentValidationAutoValidation()
            .RequireFeatureToggle(ReleaseFlags.EnableKnowledgeManagement)
            .WithTags("Knowledges")
            .WithOpenApi();

        endpointGroup
            .MapGet("", GetKnowledgesEndpoint.GetKnowledges)
            .WithSummary("Returns all knowledges.");

        endpointGroup
            .MapPost("", CreateKnowledgeEndpoint.CreateKnowledge)
            .RequirePolicyAuthorization(Policies.ManageKnowledges);
        
        endpointGroup
            .MapPut("{id}", EditKnowledgeEndpoint.EditKnowledges)
            .RequirePolicyAuthorization(Policies.ManageKnowledges);
         
        endpointGroup
            .MapDelete("{id}", DeleteKnowledgeEndpoint.DeleteKnowledge)
            .RequirePolicyAuthorization(Policies.ManageKnowledges)
            .WithSummary("Deletes the knowledge");
        
    }
}
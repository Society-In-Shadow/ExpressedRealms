using ExpressedRealms.FeatureFlags;
using ExpressedRealms.Knowledges.API.KnowledgeSpecializations.Create;
using ExpressedRealms.Knowledges.API.KnowledgeSpecializations.Delete;
using ExpressedRealms.Knowledges.API.KnowledgeSpecializations.Edit;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace ExpressedRealms.Knowledges.API.KnowledgeSpecializations;

internal static class CharacterKnowledgeSpecializationsEndpoints
{
    internal static void AddCharacterKnowledgeSpecializationEndpoints(this WebApplication app)
    {
        var endpointGroup = app.MapGroup("characters")
            .AddFluentValidationAutoValidation()
            .RequireFeatureToggle(ReleaseFlags.ShowCharacterKnowledgePage)
            .WithTags("Character Knowledge Specializations")
            .WithOpenApi();

        endpointGroup.MapPost(
            "{characterid}/knowledges/{mappingId}/specialization",
            CreateKnowledgeSpecializationEndpoint.Create
        );

        endpointGroup.MapPut(
            "{characterId}/knowledges/{mappingId}/specialization/{specializationId}",
            EditCharacterSpecializationEndpoint.Edit
        );

        endpointGroup.MapDelete(
            "{characterId}/knowledges/{mappingId}/specialization/{specializationId}",
            DeleteCharacterSpecializationEndpoint.Delete
        );
    }
}

using ExpressedRealms.Knowledges.API.KnowledgeSpecializations.Create;
using ExpressedRealms.Knowledges.API.KnowledgeSpecializations.Delete;
using ExpressedRealms.Knowledges.API.KnowledgeSpecializations.Edit;
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

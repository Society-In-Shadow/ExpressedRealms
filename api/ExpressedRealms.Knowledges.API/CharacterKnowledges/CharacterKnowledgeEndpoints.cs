using ExpressedRealms.Knowledges.API.CharacterKnowledges.Create;
using ExpressedRealms.Knowledges.API.CharacterKnowledges.Delete;
using ExpressedRealms.Knowledges.API.CharacterKnowledges.Edit;
using ExpressedRealms.Knowledges.API.CharacterKnowledges.GetAll;
using ExpressedRealms.Knowledges.API.CharacterKnowledges.GetOptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace ExpressedRealms.Knowledges.API.CharacterKnowledges;

internal static class CharacterKnowledgeEndpoints
{
    internal static void AddCharacterKnowledgeEndpoints(this WebApplication app)
    {
        var endpointGroup = app.MapGroup("characters")
            .AddFluentValidationAutoValidation()
            .WithTags("Character Knowledges")
            .WithOpenApi();

        endpointGroup
            .MapGet("{characterId}/knowledges", GetCharacterKnowledgesEndpoint.GetKnowledges)
            .WithSummary("Returns all knowledges.");

        endpointGroup.MapPost(
            "{characterid}/knowledges",
            CreateKnowledgeMappingEndpoint.CreateMapping
        );

        endpointGroup.MapGet(
            "{characterid}/knowledges/options",
            GetCharacterKnowledgeOptions.CharacterKnowledgeOptions
        );

        endpointGroup.MapPut(
            "{characterId}/knowledges/{mappingId}",
            EditCharacterKnowledgeEndpoint.EditKnowledges
        );

        endpointGroup.MapDelete(
            "{characterId}/knowledges/{mappingId}",
            DeleteCharacterKnowledgeEndpoint.DeleteMapping
        );
    }
}

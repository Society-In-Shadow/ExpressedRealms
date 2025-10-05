using ExpressedRealms.Blessings.API.CharacterBlessings.Create;
using ExpressedRealms.Blessings.API.CharacterBlessings.Delete;
using ExpressedRealms.Blessings.API.CharacterBlessings.Edit;
using ExpressedRealms.Blessings.API.CharacterBlessings.GetAll;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace ExpressedRealms.Blessings.API.CharacterBlessings;

internal static class CharacterBlessingsEndpoints
{
    internal static void AddCharacterBlessingsEndpoints(this WebApplication app)
    {
        var endpointGroup = app.MapGroup("characters")
            .AddFluentValidationAutoValidation()
            .WithTags("Character Blessings")
            .WithOpenApi();

        endpointGroup.MapGet("{characterId}/blessings", GetCharacterBlessingsEndpoint.ExecuteAsync);

        endpointGroup.MapPost(
            "{characterid}/blessings",
            CreateBlessingMappingEndpoint.ExecuteAsync
        );

        endpointGroup.MapPut(
            "{characterId}/blessings/{mappingId}",
            EditCharacterBlessingEndpoint.ExecuteAsync
        );

        endpointGroup.MapDelete(
            "{characterId}/blessings/{mappingId}",
            DeleteBlessingKnowledgeEndpoint.ExecuteAsync
        );
    }
}

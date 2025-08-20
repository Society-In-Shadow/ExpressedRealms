using ExpressedRealms.Powers.API.CharacterPowerEndpoints.Create;
using ExpressedRealms.Powers.API.CharacterPowerEndpoints.Delete;
using ExpressedRealms.Powers.API.CharacterPowerEndpoints.Edit;
using ExpressedRealms.Powers.API.CharacterPowerEndpoints.GetAll;
using ExpressedRealms.Powers.API.CharacterPowerEndpoints.GetPickable;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace ExpressedRealms.Powers.API.CharacterPowerEndpoints;

internal static class CharacterPowersEndpoints
{
    internal static void AddCharacterPowerApi(this WebApplication app)
    {
        var endpointGroup = app.MapGroup("characters")
            .AddFluentValidationAutoValidation()
            .WithTags("Character Powers")
            .WithOpenApi();

        endpointGroup
            .MapGet("{characterId}/powers", GetCharacterPowersEndpoint.GetPowers)
            .WithSummary("Returns character powers.");
        
        endpointGroup
            .MapGet("{characterId}/pickablepowers", GetCharacterPickablePowersEndpoint.GetPowers)
            .WithSummary("Returns powers available to the user.");

        endpointGroup.MapPost(
            "{characterid}/powers",
            CreatePowerMappingEndpoint.CreateMapping
        );

        /*endpointGroup.MapGet(
            "{characterid}/powers/options",
            GetCharacterPowerOptions.CharacterPOwerOptions
        );*/

        endpointGroup.MapPut(
            "{characterId}/powers/{mappingId}",
            EditCharacterPowerEndpoint.EditKnowledges
        );

        endpointGroup.MapDelete(
            "{characterId}/powers/{mappingId}",
            DeleteCharacterPowerEndpoint.DeleteMapping
        );
    }
}
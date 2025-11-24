using ExpressedRealms.Powers.API.CharacterPowerEndpoints.Create;
using ExpressedRealms.Powers.API.CharacterPowerEndpoints.Delete;
using ExpressedRealms.Powers.API.CharacterPowerEndpoints.Edit;
using ExpressedRealms.Powers.API.CharacterPowerEndpoints.GetAll;
using ExpressedRealms.Powers.API.CharacterPowerEndpoints.GetOptions;
using ExpressedRealms.Powers.API.CharacterPowerEndpoints.GetPickable;
using ExpressedRealms.Powers.API.CharacterPowerEndpoints.GetPowerCards;
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
            .WithTags("Character Powers");

        endpointGroup
            .MapGet("{characterId}/powers", GetCharacterPowersEndpoint.GetPowers)
            .WithSummary("Returns character powers.");

        endpointGroup
            .MapGet("{characterId}/pickablepowers", GetCharacterPickablePowersEndpoint.GetPowers)
            .WithSummary("Returns powers available to the user.");

        endpointGroup.MapPost("{characterid}/powers", CreatePowerMappingEndpoint.CreateMapping);

        endpointGroup.MapGet(
            "{characterId}/powers/{powerId}/options",
            GetCharacterPowerOptionsEndpoint.GetOptions
        );

        endpointGroup.MapGet(
            "{characterId}/powers/downloadCards",
            GetCharacterPowerCardReportEndpoint.Execute
        );

        endpointGroup.MapPut(
            "{characterId}/powers/{powerId}",
            EditCharacterPowerEndpoint.EditKnowledges
        );

        endpointGroup.MapDelete(
            "{characterId}/powers/{powerId}",
            DeleteCharacterPowerEndpoint.DeleteMapping
        );
    }
}

using ExpressedRealms.Characters.API.ContactEndpoints.Create;
using ExpressedRealms.Characters.API.ContactEndpoints.Delete;
using ExpressedRealms.Characters.API.ContactEndpoints.Edit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace ExpressedRealms.Characters.API.ContactEndpoints;

internal static class CharacterContactEndpoints
{
    internal static void AddCharacterContactEndpoints(this WebApplication app)
    {
        var endpointGroup = app.MapGroup("characters")
            .AddFluentValidationAutoValidation()
            .WithTags("Character Contacts")
            .RequireAuthorization();

        endpointGroup.MapPost("{characterId}/contacts", CreateEndpoint.ExecuteAsync);
        endpointGroup.MapPut("{characterId}/contacts/{contactId}", EditEndpoint.ExecuteAsync);
        endpointGroup.MapDelete("{characterId}/contacts/{contactId}", DeleteEndpoint.ExecuteAsync);
    }
}

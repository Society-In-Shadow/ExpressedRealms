using ExpressedRealms.Authentication.PermissionCollection;
using ExpressedRealms.Authentication.PermissionCollection.Configuration;
using ExpressedRealms.Characters.API.ContactEndpoints.Approve;
using ExpressedRealms.Characters.API.ContactEndpoints.Create;
using ExpressedRealms.Characters.API.ContactEndpoints.Delete;
using ExpressedRealms.Characters.API.ContactEndpoints.Edit;
using ExpressedRealms.Characters.API.ContactEndpoints.GetContact;
using ExpressedRealms.Characters.API.ContactEndpoints.GetContacts;
using ExpressedRealms.Characters.API.ContactEndpoints.GetContactsForCharacterSheet;
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

        endpointGroup.MapGet("{characterId}/contacts", GetContactsEndpoint.ExecuteAsync);
        endpointGroup.MapGet("{characterId}/contacts/characterSheet", GetContactsForCharacterSheetEndpoint.ExecuteAsync);
        endpointGroup.MapGet("{characterId}/contacts/{contactId}", GetContactEndpoint.ExecuteAsync);
        endpointGroup.MapPost("{characterId}/contacts", CreateEndpoint.ExecuteAsync);
        endpointGroup.MapPut("{characterId}/contacts/{contactId}", EditEndpoint.ExecuteAsync);
        endpointGroup.MapDelete("{characterId}/contacts/{contactId}", DeleteEndpoint.ExecuteAsync);
        endpointGroup
            .MapPut("{characterId}/contacts/{contactId}/approve", ApproveEndpoint.ExecuteAsync)
            .RequirePermission(Permissions.CharacterContacts.Approve);
    }
}

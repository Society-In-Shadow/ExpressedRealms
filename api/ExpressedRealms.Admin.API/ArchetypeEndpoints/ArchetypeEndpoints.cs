using ExpressedRealms.Admin.API.ArchetypeEndpoints.DeleteArchetype;
using ExpressedRealms.Admin.API.ArchetypeEndpoints.GetArchetypes;
using ExpressedRealms.Authentication.PermissionCollection;
using ExpressedRealms.Authentication.PermissionCollection.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace ExpressedRealms.Admin.API.ArchetypeEndpoints;

public static class ArchetypeEndpoints
{
    internal static void AddAdminArchetypeEndPoints(this WebApplication app)
    {
        var endpointGroup = app.MapGroup("admin")
            .AddFluentValidationAutoValidation()
            .WithTags("Admin - Archetypes");

        endpointGroup
            .MapGet("archetypes", GetArchetypeListEndpoint.Execute)
            .RequirePermission(Permissions.Archetypes.View);
        
        endpointGroup
            .MapDelete("archetypes/{id}", DeleteArchetypeEndpoint.ExecuteAsync)
            .RequirePermission(Permissions.Archetypes.Delete);
        
    }
}

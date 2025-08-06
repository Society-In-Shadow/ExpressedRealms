using ExpressedRealms.Knowledges.API.CharacterKnowledges;
using ExpressedRealms.Knowledges.API.KnowledgeSpecializations;
using Microsoft.AspNetCore.Builder;

namespace ExpressedRealms.Knowledges.API.Configuration;

public static class KnowledgesApiConfiguration
{
    public static void ConfigureKnowledgeEndPoints(this WebApplication app)
    {
        app.AddKnowledgeEndpoints();
        app.AddKnowledgeTypesEndpoints();
        app.AddCharacterKnowledgeEndpoints();
        app.AddCharacterKnowledgeSpecializationEndpoints();
    }
}

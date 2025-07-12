using Microsoft.AspNetCore.Builder;

namespace ExpressedRealms.Knowledges.API.Configuration;

public static class KnowledgesApiConfiguration
{
    public static void ConfigureKnowledgeEndPoints(this WebApplication app)
    {
        app.AddKnowledgeEndpoints();
        app.AddKnowledgeTypesEndpoints();
    }
}

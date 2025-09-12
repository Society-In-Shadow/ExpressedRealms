using ExpressedRealms.Blessings.API.BlessingLevels;
using ExpressedRealms.Blessings.API.Blessings;
using ExpressedRealms.Blessings.API.CharacterBlessings;
using Microsoft.AspNetCore.Builder;

namespace ExpressedRealms.Blessings.API.Configuration;

public static class BlessingsApiConfiguration
{
    public static void ConfigureBlessingEndPoints(this WebApplication app)
    {
        app.AddBlessingEndpoints();
        app.AddBlessingLevelEndpoints();
        app.AddCharacterBlessingsEndpoints();
    }
}

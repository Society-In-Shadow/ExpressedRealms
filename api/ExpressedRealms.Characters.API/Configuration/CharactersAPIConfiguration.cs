using ExpressedRealms.Characters.API.CharacterEndPoints;
using Microsoft.AspNetCore.Builder;

namespace ExpressedRealms.Characters.API.Configuration;

public static class CharactersAPIConfiguration
{
    public static void AddCharacterAPIEndPoints(this WebApplication app)
    {
        app.AddCharacterEndPoints();
        app.AddStatEndPoints();
    }
}

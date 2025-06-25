using ExpressedRealms.Characters.API.CharacterEndPoints;
using ExpressedRealms.Characters.API.StatEndPoints;
using Microsoft.AspNetCore.Builder;

namespace ExpressedRealms.Characters.API.Configuration;

public static class CharactersApiConfiguration
{
    public static void AddCharacterApiEndPoints(this WebApplication app)
    {
        app.AddCharacterEndPoints();
        app.AddStatEndPoints();
    }
}

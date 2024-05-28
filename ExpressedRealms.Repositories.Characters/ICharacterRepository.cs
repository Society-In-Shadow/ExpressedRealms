using ExpressedRealms.Repositories.Characters.DTOs;

namespace ExpressedRealms.Repositories.Characters;

public interface ICharacterRepository
{
    Task<List<CharacterListDTO>> GetCharactersAsync();
}
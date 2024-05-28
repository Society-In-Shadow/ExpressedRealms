using ExpressedRealms.Repositories.Characters.DTOs;
using FluentResults;

namespace ExpressedRealms.Repositories.Characters;

public interface ICharacterRepository
{
    Task<List<CharacterListDTO>> GetCharactersAsync();
    Task<GetEditCharacterDto?> GetCharacterInfoAsync(int id);
    Task<Result<int>> CreateCharacter(AddCharacterDto characterDto);
}
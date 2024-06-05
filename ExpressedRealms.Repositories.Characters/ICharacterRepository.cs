using ExpressedRealms.Repositories.Characters.DTOs;
using FluentResults;

namespace ExpressedRealms.Repositories.Characters;

public interface ICharacterRepository
{
    Task<List<CharacterListDto>> GetCharactersAsync();
    Task<Result<GetEditCharacterDto>> GetCharacterInfoAsync(int id);
    Task<Result<int>> CreateCharacterAsync(AddCharacterDto characterDto);
    Task<Result> DeleteCharacterAsync(int id);
    Task<Result> UpdateCharacterAsync(EditCharacterDto dto);
}

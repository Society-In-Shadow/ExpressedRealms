using ExpressedRealms.Characters.Repository.DTOs;
using FluentResults;

namespace ExpressedRealms.Characters.Repository;

public interface ICharacterRepository
{
    Task<List<CharacterListDto>> GetCharactersAsync();
    Task<Result<GetEditCharacterDto>> GetCharacterInfoAsync(int id);
    Task<Result<int>> CreateCharacterAsync(AddCharacterDto characterDto);
    Task<Result> DeleteCharacterAsync(int id);
    Task<Result> UpdateCharacterAsync(EditCharacterDto dto);
    Task<bool> CharacterExistsAsync(int id);
    Task<CharacterStatusDto> GetCharacterState(int id);
}

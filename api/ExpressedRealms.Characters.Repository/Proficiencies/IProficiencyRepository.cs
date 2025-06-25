using ExpressedRealms.Characters.Repository.DTOs;
using ExpressedRealms.Characters.Repository.Proficiencies.DTOs;
using FluentResults;

namespace ExpressedRealms.Characters.Repository.Proficiencies;

public interface IProficiencyRepository
{
    Task<Result<List<ProficiencyDto>>> GetBasicProficiencies(int characterId);
}

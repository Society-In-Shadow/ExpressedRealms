using ExpressedRealms.Repositories.Characters.Skills.DTOs;
using FluentResults;

namespace ExpressedRealms.Repositories.Characters.Skills;

public interface ICharacterSkillRepository
{
    Task AddDefaultSkills(int characterId);
    Task<List<SkillDto>> GetCharacterSkills(int characterId);
    Task<List<SkillLevelOptionsDto>> GetSkillLevelValuesForSkillTypeId(int skillTypeId);
    Task<Result> UpdateSkillLevel(EditCharacterSkillMappingDto dto);
}

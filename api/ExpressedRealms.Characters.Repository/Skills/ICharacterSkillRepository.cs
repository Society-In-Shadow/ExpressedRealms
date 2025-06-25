using ExpressedRealms.Characters.Repository.Skills.DTOs;
using FluentResults;

namespace ExpressedRealms.Characters.Repository.Skills;

public interface ICharacterSkillRepository
{
    Task AddDefaultSkills(int characterId);
    Task<List<SkillDto>> GetCharacterSkills(int characterId);
    Task<List<SkillLevelOptionsDto>> GetSkillLevelValuesForSkillTypeId(int skillTypeId);
    Task<Result> UpdateSkillLevel(EditCharacterSkillMappingDto dto);
}

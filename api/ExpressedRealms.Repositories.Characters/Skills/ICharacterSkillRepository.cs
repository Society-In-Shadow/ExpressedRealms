using ExpressedRealms.Repositories.Characters.Skills.DTOs;

namespace ExpressedRealms.Repositories.Characters.Skills;

internal interface ICharacterSkillRepository
{
    Task AddDefaultSkills(int characterId);
    Task<List<SkillDto>> GetCharacterSkills(int characterId);
    Task<List<SkillLevelOptionsDto>> GetSkillLevelValuesForSkillTypeId(int skillTypeId);
}
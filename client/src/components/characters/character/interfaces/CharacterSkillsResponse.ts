export interface CharacterSkillsResponse {
    skillTypeId: number;
    name: string;
    description: string;
    levelId: number;
    levelName: string;
    levelDescription: string;
    skillSubTypeId: number;
    benefits: BenefitItemResponse[];
}
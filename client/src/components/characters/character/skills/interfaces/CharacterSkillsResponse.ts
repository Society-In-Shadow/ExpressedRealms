import type {BenefitItemResponse} from "@/components/characters/character/interfaces/BenefitItemResponse";

export interface CharacterSkillsResponse {
    skillTypeId: number;
    name: string;
    description: string;
    levelId: number;
    levelName: string;
    levelDescription: string;
    skillSubTypeId: number;
    xp: number;
    levelNumber: number;
}

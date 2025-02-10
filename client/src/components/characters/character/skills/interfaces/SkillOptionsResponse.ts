import type {BenefitItemResponse} from "@/components/characters/character/interfaces/BenefitItemResponse";

export interface SkillResponse {
  skillTypeId: number;
  name: string;
  description: any;
  levelId: number;
  benefits: BenefitItemResponse[];
  experienceCost: number;
}
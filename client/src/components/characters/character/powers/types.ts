import type {DetailedInformation, PrerequisiteDisplay} from "@/components/expressions/powers/types.ts";

export interface CharacterKnowledgeResponse {
    knowledges: Array<CharacterKnowledge>;
}

export interface CharacterKnowledge {
    mappingId: number;
    levelName: string;
    stoneModifier: number;
    notes: string | null;
    level: number;
    levelId: number;
    specializationCount: number;
    knowledge: Knowledge;
    specializations: Array<Specialization>
}

export interface Knowledge {
    name: string,
    description: string,
    type: string
}

export interface Specialization {
    id: number;
    name: string;
    description: string;
    notes: string | null;
}

export interface KnowledgeOptionResponse {
    knowledgeLevels: Array<KnowledgeOptions>;
    availableExperience: number;
}

export interface KnowledgeOptions {
    id: number;
    name: string;
    level: number;
    specializationCount: number;
    stoneModifier: number;
    generalXpCost: number;
    totalGeneralXpCost: number;
    unknownXpCost: number;
    totalUnknownXpCost: number;
    disabled: boolean;
}

export interface CharacterPowerResponse{
    powers: PowerPath[]
}

export interface PowerPath {
    id: number;
    name: string;
    description: string;
    powers: Power[];
}

export interface Power {
    id: number;
    name: string;
    category: DetailedInformation[];
    description: string;
    gameMechanicEffect: string;
    limitation: string;
    powerDuration: DetailedInformation;
    areaOfEffect: DetailedInformation;
    powerLevel: DetailedInformation;
    powerActivationType: DetailedInformation;
    other: string;
    isPowerUse: boolean;
    cost: string;
    prerequisites: PrerequisiteDisplay | null;
    userNotes: string | null;
    requiredPower: boolean;
}
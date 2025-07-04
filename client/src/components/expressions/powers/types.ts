import type {ListItem} from "@/types/ListItem";

export interface DetailedInformation {
    name: string;
    description: string;
    id: number;
}

export interface PowerStore {
    powerPathId: number;
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
}

export interface PrerequisiteDisplay {
    requiredAmount: number;
    powers: string[]
}

export interface EditPowerResponse {
    id: number;
    name: string;
    categoryIds: number[];
    description: string;
    gameMechanicEffect: string;
    limitation: string;
    powerDurationId: number;
    areaOfEffectId: number;
    powerLevelId: number;
    powerActivationTypeId: number;
    other: string;
    isPowerUse: boolean;
    cost: string;
}

export interface EditPower {
    id: number;
    name: string;
    categories: ListItem[];
    description: string;
    gameMechanicEffect: string;
    limitation: string;
    powerDuration: ListItem;
    areaOfEffect: ListItem;
    powerLevel: ListItem;
    powerActivationType: ListItem;
    other: string;
    isPowerUse: boolean;
    cost: string;
}

export interface PowerPrerequisiteOptions {
    prerequisitePowers: ListItem[];
    requiredAmount: ListItem[];
}

export interface RawPowerPrerequisite {
    id: number;
    requiredAmount: number;
    powerIds: number[];
}

export interface PowerPrerequisite {
    powerIds: ListItem[];
    requiredAmount: ListItem;
}

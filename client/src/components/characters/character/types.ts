
export interface ExperienceBreakdownResponse {
    experience: ExperienceBreakdown[];
    availableDiscretionary: number;
    totalSpentLevelXp: number;
    totalAvailableXp: number;
}

export interface ExperienceBreakdown {
    sectionTypeId: number;
    name: string;
    total: number;
    characterCreateMax: number;
    levelXp: number;
}

export interface CalculatedExperience{
    name: string;
    sectionTypeId: number;
    requiredXp: number;
    currentOptionalXp: number;
    optionalMaxXP: number;
    availableXp: number;
    total: number,
    characterCreateMax: number;
    levelXp: number;
}
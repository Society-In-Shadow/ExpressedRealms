
export interface ExperienceBreakdownResponse {
    experience: ExperienceBreakdown[];
    availableDiscretionary: number;
}

export interface ExperienceBreakdown {
    sectionTypeId: number;
    name: string;
    total: number;
    characterCreateMax: number;
    levelXp: number;
}

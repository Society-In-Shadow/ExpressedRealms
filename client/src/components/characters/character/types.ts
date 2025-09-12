
export interface ExperienceBreakdownResponse {
    experience: ExperienceBreakdown[];
}

export interface ExperienceBreakdown {
    name: string;
    total: number;
    characterCreateMax: number;
    levelXp: number;
}

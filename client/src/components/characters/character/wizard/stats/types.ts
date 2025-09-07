export interface LevelInfo {
    bonus: number;
    description: string;
    level: number;
    totalXP: number;
    xp: number;
    disabled: boolean;
}

export interface Stat {
    availableXP: number;
    description: string;
    id: number;
    name: string;
    statLevel: number;
    statLevelInfo: LevelInfo;
}
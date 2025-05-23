
export interface DetailedInformation {
    name: string;
    description: string;
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
}

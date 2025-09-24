
export interface CharacterListResponse {
    characters: PrimaryCharacter[]
}

export interface PrimaryCharacter {
    id: string;
    name: string;
    background?: string;
    expression: string;
    playerName: string;
    assignedXp: number;
}
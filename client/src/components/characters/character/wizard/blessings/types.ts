

export interface CharacterBlessingsBaseResponse{
    blessings: Array<CharacterBlessing>;
}


export interface CharacterBlessing{
    id: number;
    blessingId: number;
    blessingLevelId: number;
    name: string;
    description: string;
    levelName: string;
    levelDescription: string;
    notes: string;
}
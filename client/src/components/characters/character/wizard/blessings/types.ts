

export interface CharacterBlessingsBaseResponse{
    blessings: Array<CharacterBlessing>;
}

export interface CharacterBlessingTypes{
    name: string;
    blessings: Array<CharacterBlessing>
}

export interface CharacterBlessing{
    type: string;
    subCategory: string;
    id: number;
    blessingId: number;
    blessingLevelId: number;
    name: string;
    description: string;
    levelName: string;
    levelDescription: string;
    notes: string;
}
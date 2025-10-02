
export interface ProgressionPathResponse{
    paths: ProgressionPath[];
}

export interface ProgressionPath{
    description: string;
    name: string;
    id: string;
    levels: ProgressionLevel[]
}

export interface ProgressionLevel{
    description: string;
    xlLevel: number;
    id: number;
    modiferGroupId: number;
}

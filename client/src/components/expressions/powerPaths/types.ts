import type {Power} from "@/components/expressions/powers/types";

export interface PowerPath {
    id: number;
    name: string;
    description: string;
    powers: Power[];
}

export interface CreatePowerPath {
    expressionId: number;
    name: string;
    description: string;
}

export interface EditPowerPath {
    id: number;
    name: string;
    description: string;
}


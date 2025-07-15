import type {ListItem} from "@/types/ListItem";

export interface Knowledge {
    id: number,
    name: string,
    description: string,
    typeName: string,
    typeDescription: string,
    typeId: number
}

export interface EditKnowledgeRequest {
    id: number,
    name: string,
    description: string,
    typeId: number
}

export interface EditKnowledge {
    id: number,
    name: string,
    description: string,
    knowledgeType: ListItem
}


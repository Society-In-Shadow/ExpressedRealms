import type {ListItem} from "@/types/ListItem";
import type {CharacterKnowledge} from "@/components/characters/character/knowledges/types.ts";

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

export interface KnowledgeGroup {
    name: string,
    knowledges: Array<Knowledge>
}

export interface CharacterKnowledgeGroup {
    name: string,
    knowledges: Array<CharacterKnowledge>
}

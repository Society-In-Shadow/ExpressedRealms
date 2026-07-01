import type { ListItem } from '@/types/ListItem.ts'

export interface FactionResponse {
  factions: Array<Faction>
}

export interface Faction {
  id: number
  name: string
  background: string
}

export interface EditSingleFactionInfo {
  name: string
  background: string
}

export interface CreateSingleFactionInfo {
  name: string
  background: string
  knowledge: ListItem
  specialization: string
}

export interface CreateSingleFactionPost {
  name: string
  background: string
  expressionId: number
  knowledgeId: number
  specialization: string
}

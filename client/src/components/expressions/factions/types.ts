import type { ListItem } from '@/types/ListItem.ts'
import type { Power } from '@/components/expressions/powers/types.ts'

export interface FactionResponse {
  factions: Array<Faction>
}

export interface Faction {
  id: number
  name: string
  background: string
  factionLevels?: FactionLevel[]
}

export interface FactionLevel {
  id: number
  rankName: string
  knowledgeId?: number | string | null
  knowledge?: string | null
  knowledgeLevel?: string | null
  specialization?: string | null
  knowledgeLevelId?: number | string | null
  power?: Power | null
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

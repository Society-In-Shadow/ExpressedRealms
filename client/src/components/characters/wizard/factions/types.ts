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

export interface PickFactionInfo {
  characterId: number
  factionId: number
}

export interface FactionLevelsResponse {
  factionLevels: FactionLevel[]
  factionId: number | null
}

export interface FactionLevel {
  factionLevelId: number
  approver: string | null
  approvalReason: string | null
  requestedPromotion: boolean
  requestedPromotionReason: string | null
  approvalDate: string | null // ISO 8601 date-time
  hasKnowledge: boolean
  hasKnowledgeLevel: boolean
  hasSpecialization: boolean | null
  characterNotes: string | null
}

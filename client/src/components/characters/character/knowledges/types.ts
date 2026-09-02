export interface CharacterKnowledgeResponse {
  knowledges: Array<CharacterKnowledge>
}

export interface CharacterKnowledge {
  mappingId: number
  levelName: string
  stoneModifier: number
  notes: string | null
  level: number
  levelId: number
  minimumKnowledgeId: number
  specializationCount: number
  knowledge: Knowledge
  specializations: Array<Specialization>
}

export interface Knowledge {
  id: number
  name: string
  description: string
  type: string
  blockFactionChanges: boolean
}

export interface Specialization {
  id: number
  name: string
  description: string
  notes: string | null
  blockFactionChanges: boolean
}

export interface KnowledgeOptionResponse {
  knowledgeLevels: Array<KnowledgeOptions>
  availableExperience: number
}

export interface KnowledgeOptions {
  id: number
  name: string
  level: number
  specializationCount: number
  stoneModifier: number
  generalXpCost: number
  totalGeneralXpCost: number
  unknownXpCost: number
  totalUnknownXpCost: number
  disabled: boolean
}

export interface KnowledgeLevel {
  id: number
  name: string
  stones: number
  level: number
  totalXpCost: number
  specializationCount: number
  isSelected: boolean
}

export interface EditKnowledge {
  id: number
  name: string
  knowledgeType: string
  description: string
  notes?: string | null
  selectedLevelId: number
  minimumKnowledgeId: number
  blockFactionChanges: boolean
  knowledgeLevels: KnowledgeLevel[]
  specializations: Array<Specialization>
}

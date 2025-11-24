import { type XpSectionType } from '@/components/characters/character/stores/experienceBreakdownStore.ts'

export interface ExperienceBreakdownResponse {
  experience: ExperienceBreakdown[]
  availableDiscretionary: number
  totalSpentLevelXp: number
  totalAvailableXp: number
}

export interface ExperienceBreakdown {
  sectionTypeId: XpSectionType
  name: string
  total: number
  characterCreateMax: number
  levelXp: number
}

export interface CalculatedExperience {
  name: string
  sectionTypeId: XpSectionType
  requiredXp: number
  currentOptionalXp: number
  optionalMaxXP: number
  availableXp: number
  total: number
  characterCreateMax: number
  levelXp: number
}

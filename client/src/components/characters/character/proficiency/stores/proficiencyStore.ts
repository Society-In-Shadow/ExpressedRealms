import { defineQueryOptions } from '@pinia/colada'
import { proficienciesService } from '@/components/characters/character/proficiency/services/proficienciesServices.ts'

export const PROFICIENCY_QUERY_KEYS = {
  root: ['proficiency'] as const,
  proficienciesById: (id: number) => ['proficiencies', id] as const,
}

export const proficienciesByCharacterId
  = defineQueryOptions((id: number) => ({
    key: PROFICIENCY_QUERY_KEYS.proficienciesById(id),
    query: () => proficienciesService.getProficiencies(id),
  }),
  )

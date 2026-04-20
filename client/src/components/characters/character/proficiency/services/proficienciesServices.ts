import axios from 'axios'
import type { ProficienciesResponse } from '@/components/characters/character/proficiency/types.ts'

export const proficienciesService = {
  getProficiencies: (characterId: number): Promise<ProficienciesResponse> => axios.get<ProficienciesResponse>(`proficiencies/${characterId}`)
    .then(async (response) => { return response.data }),
}

import axios from 'axios'
import type { FactionResponse } from '@/components/expressions/factions/types.ts'

export const factionService = {
  getFactions: (expressionId: number): Promise<FactionResponse> => axios.get<FactionResponse>(`/factions/expressions/${expressionId}`)
    .then(async (response) => { return response.data }),
  delete: (id: number) => axios.delete(`/factions/${id}`)
    .then(async (response) => { return response.data }),
}

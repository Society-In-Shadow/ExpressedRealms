import axios from 'axios'
import type {
  CreateSingleFactionPost,
  EditSingleFactionInfo,
  FactionResponse,
} from '@/components/expressions/factions/types.ts'

export const factionService = {
  getFactions: (expressionId: number): Promise<FactionResponse> => axios.get<FactionResponse>(`/factions/expressions/${expressionId}`)
    .then(async (response) => { return response.data }),
  getFaction: (id: number): Promise<EditSingleFactionInfo> => axios.get<EditSingleFactionInfo>(`/factions/${id}`)
    .then(async (response) => { return response.data }),
  editFaction: (id: number, faction: EditSingleFactionInfo): Promise<EditSingleFactionInfo> => axios.put<EditSingleFactionInfo>(`/factions/${id}`, faction)
    .then(async (response) => { return response.data }),
  createFaction: (faction: CreateSingleFactionPost): Promise<number> => axios.post<number>(`/factions/`, faction)
    .then(async (response) => { return response.data }),
  delete: (id: number) => axios.delete(`/factions/${id}`)
    .then(async (response) => { return response.data }),
}

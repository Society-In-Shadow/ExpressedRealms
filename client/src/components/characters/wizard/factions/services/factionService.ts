import axios from 'axios'
import type { FactionLevelsResponse } from '@/components/characters/wizard/factions/types.ts'

export const characterFactionService = {
  getCharacterFaction: (id: number): Promise<FactionLevelsResponse> => axios.get<FactionLevelsResponse>(`/characters/${id}/factions`)
    .then(async (response) => { return response.data }),
  pickFaction: (characterId: number, factionId: number) => axios.post(`/characters/${characterId}/factions/${factionId}`),
  leaveFaction: (characterId: number) => axios.delete(`/characters/${characterId}/factions/leave`),
  requestPromotion: (characterId: number, factionLevelId: number, requestReason: string | null) => axios.put(`/characters/${characterId}/factions/requestPromotion`, { requestReason, factionLevelId }),
  approvePromotion: (characterId: number, factionLevelId: number, approvalReason: string | null) => axios.put(`/characters/${characterId}/factions/approvePromotion`, { approvalReason, factionLevelId }),

}

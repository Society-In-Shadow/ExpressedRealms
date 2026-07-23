import axios from 'axios'
import type { FactionLevelsResponse } from '@/components/characters/wizard/factions/types.ts'

export const characterFactionService = {
  getCharacterFaction: (id: number): Promise<FactionLevelsResponse> => axios.get<FactionLevelsResponse>(`/characters/${id}/factions`)
    .then(async (response) => { return response.data }),
  pickFaction: (characterId: number, factionId: number) => axios.post(`/characters/${characterId}/factions/${factionId}`),
}

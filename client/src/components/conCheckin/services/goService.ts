import axios from 'axios'
import type { GoChecksResponse } from '@/components/conCheckin/types.ts'

export const goService = {
  getGoChecks: (characterId: number): Promise<GoChecksResponse> => axios.get<GoChecksResponse>(`/characters/${characterId}/goChecks`)
    .then(async (response) => { return response.data }),
}

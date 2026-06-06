import axios from 'axios'
import type { GoFields } from '@/components/admin/characterList/types.ts'

export const goFieldsService = {
  getGoFields: (characterId: number): Promise<GoFields> => axios.get<GoFields>(`/characters/${characterId}/goFields`)
    .then(async (response) => { return response.data }),
  updateGoFields: (characterId: number, goFields: GoFields) => axios.put(`/characters/${characterId}/goFields`, goFields)
    .then(async (response) => { return response.data }),
}
